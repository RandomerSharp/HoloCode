using System.Collections;
using System.Collections.Generic;

public class ProjectConfig
{
    public enum ProjectTemplate
    {
        Javascript,
        NN
    }

    public enum Console
    {
        Default,
        Text,
        Image,
        Voice
    }

    public struct Config
    {
        public ProjectTemplate type;
        public Console console;
    }
}
