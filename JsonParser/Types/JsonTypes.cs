using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonParser.Types
{
    public enum JsonTypes
    {
        JSON_NULL,
        JSON_FALSE,
        JSON_TRUE,
        JSON_NUMBER,
        JSON_STRING,
        JSON_ARRAY,
        JSON_OBJECT,
        UNKNOW = 8192
    }
}