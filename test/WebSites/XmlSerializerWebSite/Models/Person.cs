using System;

namespace XmlSerializerWebSite
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Id, Name);
        }
    }
}