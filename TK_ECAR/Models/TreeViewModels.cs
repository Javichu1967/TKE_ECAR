using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class ParentWithNoChild
    {
        public int id { get; set; }
        public string text { get; set; }
        public string[] tags
        {
            get { return new string[1] { 0.ToString() }; }
        }
    }

    public class ParentWithChildren
    {
        public int id { get; set; }
        public string text { get; set; }

        public string icon
        {
            get { return "tk-icon icon-tk-document"; }
        }
        public string href { get; set; }
        //icon: "tk-icon icon-tk-document"
        public Child[] nodes { get; set; }
        public string[] tags
        {
            get { return new string[1] { nodes.Length.ToString() }; }
        }

    }

    public class Child
    {
        public int id { get; set; }
        public string text { get; set; }
        public string href { get; set; }
        public string icon
        {
            get { return "tk-icon icon-tk-document"; }
        }
        public Child[] nodes { get; set; }
        //public string icon { get { return "tk-icon icon-tk-document"; } }
    }
}

