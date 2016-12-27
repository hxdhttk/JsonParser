using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JsonParser
{
    public static class Parser
    {
        public static ResultType JsonParse(ref JsonValue v, StreamReader json)
        {
            var c = new JsonContext { Json = json };
            v.Type = JsonTypes.JSON_NULL;
            JsonParseWhitespace(ref c);
            var ret = JsonParseValue(ref c, ref v);
            if (ret == ResultType.JSON_PARSE_OK)
            {
                JsonParseWhitespace(ref c);
                if (c.Json.EndOfStream != true)
                {
                    return ResultType.JSON_PARSE_ROOT_NOT_SINGULAR;
                }
            }

            return ret;
        }

        public static JsonTypes GetJsonType(ref JsonValue v)
        {
            return v.Type;
        }

        public static void JsonParseWhitespace(ref JsonContext c)
        {
            var p = c.Json;
            while (p.Peek() == ' ' || p.Peek() == '\t' || p.Peek() == '\n' || p.Peek() == '\r')
                p.Read();
            c.Json = p;
        }

        public static ResultType JsonParseLiteral(ref JsonContext c, ref JsonValue v, string typeString, JsonTypes type)
        {
            using (var iter = typeString.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    if (c.Json.Peek() != iter.Current)
                    {
                        return ResultType.JSON_PARSE_INVALID_VALUE;
                    }
                    c.Json.Read();
                }
            }
            v.Type = type;
            return ResultType.JSON_PARSE_OK;
        }

        public static ResultType JsonParseValue(ref JsonContext c, ref JsonValue v)
        {
            if (c.Json.EndOfStream != true)
            {

                switch (c.Json.Peek())
                {
                    case 'n':
                        return JsonParseLiteral(ref c, ref v, "null", JsonTypes.JSON_NULL);
                    case 't':
                        return JsonParseLiteral(ref c, ref v, "true", JsonTypes.JSON_TRUE);
                    case 'f':
                        return JsonParseLiteral(ref c, ref v, "false", JsonTypes.JSON_FALSE);
                    default:
                        return ResultType.JSON_PARSE_INVALID_VALUE;
                }
            }
            return ResultType.JSON_PARSE_EXCEPT_VALUE;
        }

        public class JsonContext
        {
            public StreamReader Json { get; set; }
        }

        public class JsonValue
        {
            public JsonTypes Type { get; set; }
        }

        public enum JsonTypes
        {
            JSON_NULL,
            JSON_FALSE,
            JSON_TRUE,
            JSON_NUMBER,
            JSON_STRING,
            JSON_ARRAY,
            JSON_OBJECT
        }

        public enum ResultType
        {
            JSON_PARSE_OK = 0,
            JSON_PARSE_EXCEPT_VALUE,
            JSON_PARSE_INVALID_VALUE,
            JSON_PARSE_ROOT_NOT_SINGULAR
        }
    }
}
