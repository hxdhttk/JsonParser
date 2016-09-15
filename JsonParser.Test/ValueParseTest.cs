using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonParser;
using JsonParser.Types;

namespace JsonParser.Test
{
    [TestClass]
    public unsafe class ValueParseTest
    {
        [TestMethod]
        public void TestParseNull()
        {
            var v = new JsonValue {Type = JsonTypes.UNKNOW};
            JsonValue* vPtr = &v;
            
            Assert.AreEqual(JsonParseResult.JSON_PARSE_OK, Parser.JsonParser(vPtr, (char*)Marshal.StringToBSTR("null")));
            Assert.AreEqual(JsonTypes.JSON_NULL, Parser.GetJsonType(vPtr));
        }

        [TestMethod]
        public void TestParseTrue()
        {
            var v = new JsonValue { Type = JsonTypes.UNKNOW };
            JsonValue* vPtr = &v;

            Assert.AreEqual(JsonParseResult.JSON_PARSE_OK, Parser.JsonParser(vPtr, (char*)Marshal.StringToBSTR("true")));
            Assert.AreEqual(JsonTypes.JSON_TRUE, Parser.GetJsonType(vPtr));
        }

        [TestMethod]
        public void TestParseFalse()
        {
            var v = new JsonValue { Type = JsonTypes.UNKNOW };
            JsonValue* vPtr = &v;

            Assert.AreEqual(JsonParseResult.JSON_PARSE_OK, Parser.JsonParser(vPtr, (char*)Marshal.StringToBSTR("false")));
            Assert.AreEqual(JsonTypes.JSON_FALSE, Parser.GetJsonType(vPtr));
        }

        [TestMethod]
        public void TestParseWhitespace()
        {
            var v = new JsonValue { Type = JsonTypes.UNKNOW };
            JsonValue* vPtr = &v;

            Assert.AreEqual(JsonParseResult.JSON_PARSE_EXCEPT_VALUE, Parser.JsonParser(vPtr, (char*)Marshal.StringToBSTR(" \n\r\t")));
            Assert.AreEqual(JsonTypes.JSON_NULL, Parser.GetJsonType(vPtr));
        }

        [TestMethod]
        public void TestParseSomeInvalidValues()
        {
            var v = new JsonValue { Type = JsonTypes.UNKNOW };
            JsonValue* vPtr = &v;

            Assert.AreEqual(JsonParseResult.JSON_PARSE_INVALID_VALUE, Parser.JsonParser(vPtr, (char*)Marshal.StringToBSTR("none")));
            Assert.AreEqual(JsonTypes.JSON_NULL, Parser.GetJsonType(vPtr));

            Assert.AreEqual(JsonParseResult.JSON_PARSE_INVALID_VALUE, Parser.JsonParser(vPtr, (char*)Marshal.StringToBSTR("ture")));
            Assert.AreEqual(JsonTypes.JSON_NULL, Parser.GetJsonType(vPtr));

            Assert.AreEqual(JsonParseResult.JSON_PARSE_INVALID_VALUE, Parser.JsonParser(vPtr, (char*)Marshal.StringToBSTR("flase")));
            Assert.AreEqual(JsonTypes.JSON_NULL, Parser.GetJsonType(vPtr));
        }
    }
}
