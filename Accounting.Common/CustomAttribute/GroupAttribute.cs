using System;

namespace Accounting.Common.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GroupAttribute : Attribute
    {
        public string GroupName { get; set; }
        public string Name { get; set; }

        public GroupAttribute(string name, string groupName)
        {
            GroupName = groupName;
            Name = name;
        }
    }
}
