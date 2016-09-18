using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JsonParser.Types;

namespace JsonParser
{
    public static unsafe class Parser
    {
        public static JsonParseResult JsonParser(JsonValue* v, char* json)
        {
            JsonContext c = new JsonContext();
            UnsafeAssert.Assert(() => v != null, null);
            c.Json = json;
            v->Type = JsonTypes.JSON_NULL;
            JsonParseWhitespace(&c);
            var ret = JsonParseValue(&c, v);
            if (ret == JsonParseResult.JSON_PARSE_OK)
            {
                JsonParseWhitespace(&c);
                if (*(c.Json) != '\0')
                {
                    return JsonParseResult.JSON_PARSE_ROOT_NOT_SINGULAR;
                }
            }

            return ret;
        }

        public static JsonTypes GetJsonType(JsonValue* v)
        {
            UnsafeAssert.Assert(() => v != null, null);
            return v->Type;
        }

        public static void JsonParseWhitespace(JsonContext* c)
        {
            var p = c->Json;
            while (*p == ' ' || *p == '\t' || *p == '\n' || *p == '\r')
                p++;
            c->Json = p;
        }
        
        public static JsonParseResult JsonParseLiteral(JsonContext* c, JsonValue* v, char* typeString, JsonTypes type)
        {
            while (*typeString != '\0')
            {
                if (*(c->Json) != *typeString)
                {
                    return JsonParseResult.JSON_PARSE_INVALID_VALUE;
                }
                c->Json++;
                typeString++;
            }
            v->Type = type;
            return JsonParseResult.JSON_PARSE_OK;
        }

        public static JsonParseResult JsonParseValue(JsonContext* c, JsonValue* v)
        {
            switch (*(c->Json))
            {
                case 'n':
                    return JsonParseLiteral(c, v, (char*)Marshal.StringToBSTR("null"), JsonTypes.JSON_NULL);
                case 't':
                    return JsonParseLiteral(c, v, (char*)Marshal.StringToBSTR("true"), JsonTypes.JSON_TRUE);
                case 'f':
                    return JsonParseLiteral(c, v, (char*)Marshal.StringToBSTR("false"), JsonTypes.JSON_FALSE);
                case '\0':
                    return JsonParseResult.JSON_PARSE_EXCEPT_VALUE;
                default:
                    return JsonParseResult.JSON_PARSE_INVALID_VALUE;
            }
        }
    }

    public enum JsonParseResult
    {
        JSON_PARSE_OK = 0,
        JSON_PARSE_EXCEPT_VALUE,
        JSON_PARSE_INVALID_VALUE,
        JSON_PARSE_ROOT_NOT_SINGULAR
    }

    public static unsafe class UnsafeAssert
    {
        private static Action defaultAction = () =>
        {
            throw new AssertException();
        };

        public static void Assert(Func<bool> func, Action action, Action otherOperation = null)
        {
            if (!func())
            {
                action?.Invoke();
                defaultAction();
            }

            otherOperation?.Invoke();
        }
    }

    public class AssertException : Exception
    {
        public AssertException() : base("Assertion found!")
        {

        }
    }
}
