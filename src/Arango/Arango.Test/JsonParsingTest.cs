﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arango.Client;

namespace Arango.Test
{
    [TestClass]
    public class JsonParsingTest
    {
        private JsonParser _parser = new JsonParser();

        #region Serialization



        #endregion

        #region Deserialization

        [TestMethod]
        public void DeserializeEmptyObject()
        {
            dynamic result = _parser.Deserialize("{}");
            Assert.AreEqual(0, ((IDictionary<string, object>)result).Count);
        }

        [TestMethod]
        public void DeserializeEmptyArray()
        {
            dynamic result = _parser.Deserialize("[]");
            Assert.AreEqual(0, ((IList<dynamic>)result).Count);
        }

        [TestMethod]
        public void DeserializeObjectOneMember()
        {
            dynamic result = _parser.Deserialize("{\"Foo\" :  123}");
            Assert.AreEqual(123.0, result.Foo);
        }

        [TestMethod]
        public void DeserializeArrayOneMember()
        {
            dynamic result = _parser.Deserialize("[\"Foo\"]");
            Assert.AreEqual("Foo", result[0]);
        }

        [TestMethod]
        public void DeserializeObjectStringMembers()
        {
            dynamic result = _parser.Deserialize("{\"Foo\": \"foo\", \"Bar\": \"bar\"}");
            Assert.AreEqual("foo", result.Foo);
            Assert.AreEqual("bar", result.Bar);
        }

        [TestMethod]
        public void DeserializeObjectNumberMembers()
        {
            dynamic result = _parser.Deserialize("{\"Foo\" :123, \"Bar\": 543}");
            Assert.AreEqual(123.0, result.Foo);
            Assert.AreEqual(543.0, result.Bar);
        }

        [TestMethod]
        public void DeserializeObjectArrayMembers()
        {
            dynamic result = _parser.Deserialize("{\"Foo\": [\"foo1\", \"foo2\"], \"Bar\": [\"bar1\"]}");
            CollectionAssert.AreEqual(new List<string> { "foo1", "foo2" }, result.Foo);
            CollectionAssert.AreEqual(new List<string> { "bar1" }, result.Bar);
        }

        [TestMethod]
        public void DeserializeObjectInObject()
        {
            dynamic result = _parser.Deserialize("{ \"Foo\":{\"Bar\":123}}");
            Assert.AreEqual(123.0, result.Foo.Bar);
        }

        [TestMethod]
        public void DeserializeArray()
        {
            dynamic result = _parser.Deserialize("[123, \"Foo\", \"Bar\", 321]");
            CollectionAssert.AreEqual(new List<dynamic> { 123d, "Foo", "Bar", 321d }, result);
        }

        [TestMethod]
        public void DeserializeObjectEmptyName()
        {
            dynamic result = _parser.Deserialize("{\"\" : 123 }");
        }

        [TestMethod]
        public void DeserializeObjectEmptyNameAndEmptyString()
        {
            dynamic result = _parser.Deserialize("{\"\": \"\"}");
        }

        [TestMethod]
        public void DeserializeSpecialWords()
        {
            dynamic result = _parser.Deserialize("{\"Foo\" : true, \"Bar\":false , \"FooBar\" : null}");
            Assert.AreEqual(true, result.Foo);
            Assert.AreEqual(false, result.Bar);
            Assert.AreEqual(null, result.FooBar);
        }

        [TestMethod]
        public void DeserializeNumbersCheck()
        {
            dynamic result = _parser.Deserialize("[123, -123, 123.5, -123.5, 12e2, 12E2, 12e+2, 12e-2, -12e+2]");
            Assert.AreEqual(123.0, result[0]);
        }

        [TestMethod]
        public void DeserializeRfc4627ExampleCheck()
        {
            string json = "{\n\"Image\": {\n\"Width\":  800,\n\"Height\": 600,\n\"Title\":  \"View from 15th Floor\",\n\"Thumbnail\": {\n\"Url\":    \"http://www.example.com/image/481989943\",\n\"Height\": 125,\n\"Width\":  \"100\"\n},\"IDs\": [116, 943, 234, 38793]\n}}";
            dynamic result = _parser.Deserialize(json);
            Assert.AreEqual(800, result.Image.Width);
            Assert.AreEqual(600, result.Image.Height);
            Assert.AreEqual("View from 15th Floor", result.Image.Title);
            Assert.AreEqual("http://www.example.com/image/481989943", result.Image.Thumbnail.Url);
            Assert.AreEqual(125, result.Image.Thumbnail.Height);
            Assert.AreEqual("100", result.Image.Thumbnail.Width);
            CollectionAssert.AreEqual(new List<double> { 116, 943, 234, 38793 }, result.Image.IDs);
        }

        #endregion
    }
}
