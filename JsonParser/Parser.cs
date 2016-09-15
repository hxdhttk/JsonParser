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
            if (v == null)
            {
                return JsonParseResult.JSON_PARSE_INVALID_VALUE;
            }
            c.Json = json;
            v->Type = JsonTypes.JSON_NULL;
            JsonParseWhitespace(&c);
            return JsonParseValue(&c, v);
        }

        public static JsonTypes GetJsonType(JsonValue* v)
        {
            return v->Type;
        }

        public static void JsonParseWhitespace(JsonContext* c)
        {
            var p = c->Json;
            while (*p == ' ' || *p == '\t' || *p == '\n' || *p == '\r')
                p++;
            c->Json = p;
        }

        public static JsonParseResult JsonParseNull(JsonContext* c, JsonValue* v)
        {
            if (*(c->Json) == 'n')
            {
                if (c->Json[1] != 'u' || c->Json[2] != 'l' || c->Json[3] != 'l')
                    return JsonParseResult.JSON_PARSE_INVALID_VALUE;
                c->Json += 3;
                v->Type = JsonTypes.JSON_NULL;
            }

            return JsonParseResult.JSON_PARSE_OK;
        }

        public static JsonParseResult JsonParseTrue(JsonContext* c, JsonValue* v)
        {
            if (*(c->Json) == 't')
            {
                if (c->Json[1] != 'r' || c->Json[2] != 'u' || c->Json[3] != 'e')
                    return JsonParseResult.JSON_PARSE_INVALID_VALUE;
                c->Json += 3;
                v->Type = JsonTypes.JSON_TRUE;
            }

            return JsonParseResult.JSON_PARSE_OK;
        }

        public static JsonParseResult JsonParseFalse(JsonContext* c, JsonValue* v)
        {
            if (*(c->Json) == 'f')
            {
                if (c->Json[1] != 'a' || c->Json[2] != 'l' || c->Json[3] != 's' || c->Json[4] != 'e')
                    return JsonParseResult.JSON_PARSE_INVALID_VALUE;
                c->Json += 4;
                v->Type = JsonTypes.JSON_FALSE;
            }

            return JsonParseResult.JSON_PARSE_OK;
        }

        public static JsonParseResult JsonParseValue(JsonContext* c, JsonValue* v)
        {
            switch (*(c->Json))
            {
                case 'n':
                    return JsonParseNull(c, v);
                case 't':
                    return JsonParseTrue(c, v);
                case 'f':
                    return JsonParseFalse(c, v);
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
}
