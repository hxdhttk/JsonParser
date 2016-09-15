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
            return JsonParseValue(&c, v);
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

        public static JsonParseResult JsonParseNull(JsonContext* c, JsonValue* v)
        {
            UnsafeAssert.Assert(() => c->Json[0] == 'n', null, () => c->Json++);
            if (c->Json[0] != 'u' || c->Json[1] != 'l' || c->Json[2] != 'l')
                return JsonParseResult.JSON_PARSE_INVALID_VALUE;
            c->Json += 3;
            v->Type = JsonTypes.JSON_NULL;

            return JsonParseResult.JSON_PARSE_OK;
        }

        public static JsonParseResult JsonParseTrue(JsonContext* c, JsonValue* v)
        {
            UnsafeAssert.Assert(() => c->Json[0] == 't', null, () => c->Json++);
            if (c->Json[0] != 'r' || c->Json[1] != 'u' || c->Json[2] != 'e')
                return JsonParseResult.JSON_PARSE_INVALID_VALUE;
            c->Json += 3;
            v->Type = JsonTypes.JSON_TRUE;

            return JsonParseResult.JSON_PARSE_OK;
        }

        public static JsonParseResult JsonParseFalse(JsonContext* c, JsonValue* v)
        {
            UnsafeAssert.Assert(() => c->Json[0] == 'f', null, () => c->Json++);
            if (c->Json[0] != 'a' || c->Json[1] != 'l' || c->Json[2] != 's' || c->Json[3] != 'e')
                return JsonParseResult.JSON_PARSE_INVALID_VALUE;
            c->Json += 4;
            v->Type = JsonTypes.JSON_FALSE;

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
