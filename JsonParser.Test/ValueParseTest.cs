using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonParser;

namespace JsonParser.Test
{
    [TestClass]
    public class ValueParseTest
    {
        [TestMethod]
        public void TestParseNull()
        {
            var v = new Parser.JsonValue { Type = Parser.JsonTypes.JSON_NULL };
            var json = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("null")));

            Assert.AreEqual(Parser.ResultType.JSON_PARSE_OK, Parser.JsonParse(ref v, json));
            Assert.AreEqual(Parser.JsonTypes.JSON_NULL, Parser.GetJsonType(ref v));
        }

        [TestMethod]
        public void TestParseTrue()
        {
            var v = new Parser.JsonValue { Type = Parser.JsonTypes.JSON_NULL };
            var json = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("true")));

            Assert.AreEqual(Parser.ResultType.JSON_PARSE_OK, Parser.JsonParse(ref v, json));
            Assert.AreEqual(Parser.JsonTypes.JSON_TRUE, Parser.GetJsonType(ref v));
        }

        [TestMethod]
        public void TestParseFalse()
        {
            var v = new Parser.JsonValue { Type = Parser.JsonTypes.JSON_NULL };
            var json = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("false")));

            Assert.AreEqual(Parser.ResultType.JSON_PARSE_OK, Parser.JsonParse(ref v, json));
            Assert.AreEqual(Parser.JsonTypes.JSON_FALSE, Parser.GetJsonType(ref v));
        }

        [TestMethod]
        public void TestParseWhitespace()
        {
            var v = new Parser.JsonValue { Type = Parser.JsonTypes.JSON_NULL };
            var json = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(" \n\r\t")));

            Assert.AreEqual(Parser.ResultType.JSON_PARSE_EXCEPT_VALUE, Parser.JsonParse(ref v, json));
            Assert.AreEqual(Parser.JsonTypes.JSON_NULL, Parser.GetJsonType(ref v));
        }

        [TestMethod]
        public void TestParseSomeInvalidValues()
        {
            var v = new Parser.JsonValue { Type = Parser.JsonTypes.JSON_NULL };

            Assert.AreEqual(Parser.ResultType.JSON_PARSE_INVALID_VALUE,
                Parser.JsonParse(ref v, new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("none")))));
            Assert.AreEqual(Parser.JsonTypes.JSON_NULL, Parser.GetJsonType(ref v));

            Assert.AreEqual(Parser.ResultType.JSON_PARSE_INVALID_VALUE,
                Parser.JsonParse(ref v, new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("ture")))));
            Assert.AreEqual(Parser.JsonTypes.JSON_NULL, Parser.GetJsonType(ref v));

            Assert.AreEqual(Parser.ResultType.JSON_PARSE_INVALID_VALUE,
                Parser.JsonParse(ref v, new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("flase")))));
            Assert.AreEqual(Parser.JsonTypes.JSON_NULL, Parser.GetJsonType(ref v));
        }

        [TestMethod]
        public void TestParseRootNotSingular()
        {
            var v = new Parser.JsonValue { Type = Parser.JsonTypes.JSON_NULL };

            Assert.AreEqual(Parser.ResultType.JSON_PARSE_ROOT_NOT_SINGULAR,
                Parser.JsonParse(ref v, new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("null n")))));
            Assert.AreEqual(Parser.JsonTypes.JSON_NULL, Parser.GetJsonType(ref v));
        }
    }
}
