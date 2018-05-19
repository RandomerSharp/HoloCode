using System.Collections;
using System.Collections.Generic;

public class CodeColor
{
    public static string JavaScript(string origin)
    {
        origin = origin.Replace("int", "<#0707a0ff>int</color>");
        origin = origin.Replace("using", "<#0707a0ff>using</color>");
        origin = origin.Replace("namespace", "<#0707a0ff>namespace</color>");
        origin = origin.Replace("float", "<#0707a0ff>float</color>");

        origin = origin.Replace("var", "<#0707a0ff>var</color>");
        origin = origin.Replace("let", "<#0707a0ff>let</color>");
        origin = origin.Replace("=>", "<#0707a0ff>=></color>");
        origin = origin.Replace("function", "<#0707a0ff>function</color>");
        origin = origin.Replace("null", "<#0707a0ff>null</color>");
        origin = origin.Replace("true", "<#0707a0ff>true</color>");
        origin = origin.Replace("false", "<#0707a0ff>false</color>");
        origin = origin.Replace("@param", "<#0707a0ff>@param</color>");
        origin = origin.Replace("try", "<#0707a0ff>try</color>");
        origin = origin.Replace("catch", "<#0707a0ff>catch</color>");

        origin = origin.Replace("if", "<#770077ff>if</color>");
        origin = origin.Replace("else", "<#770077ff>else</color>");
        origin = origin.Replace("return", "<#770077ff>return</color>");

        origin = origin.Replace("console", "<#07a007ff>console</color>");
        origin = origin.Replace("JSON", "<#07a007ff>JSON</color>");
        origin = origin.Replace("Error", "<#07a007ff>Error</color>");
        origin = origin.Replace("string", "<#07a007ff>string</color>");
        origin = origin.Replace("void", "<#07a007ff>void</color>");
        origin = origin.Replace("String", "<#07a007ff>String</color>");
        origin = origin.Replace("exports", "<#07a007ff>exports</color>");
        origin = origin.Replace("any", "<#07a007ff>any</color>");
        origin = origin.Replace("boolean", "<#07a007ff>boolean</color>");
        origin = origin.Replace("Math", "<#07a007ff>Math</color>");
        origin = origin.Replace("Map", "<#07a007ff>Map</color>");
        origin = origin.Replace("Array", "<#07a007ff>Array</color>");

        return origin;
    }

    public static string Python(string origin)
    {
        return origin;
    }
}
