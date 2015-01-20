using System;

namespace XmlSerializerWebSite
{
    public class PersonWrapper
    {
        public PersonWrapper() { }

        public PersonWrapper(Person person)
        {
            Id = person.Id;
            Name = person.Name;
            Age = 35;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Id, Name, Age);
        }
    }
}