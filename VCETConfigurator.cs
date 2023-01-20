using System.Collections.Generic;
using Microsoft.Build.Utilities;
using System.Xml;
using Microsoft.Build.Framework;
using System;

namespace SoupSoftware
{
    public class VCETConfigurator : Task
    {

        [Required]
        public string Path { get; set; }
        [Required]
        public string referencePath { get; set; }

        const string xmlTag = "extension";
        const string attribute = "interface";
        public override bool Execute()
        {

            if (!System.IO.Directory.Exists(Path))
            {
                Log.LogError("Input Path " + Path + " cannot be found");
                return false;
            }

            string[] vcetfiles = System.IO.Directory.GetFiles(Path, "*.VCET.config");

            if (vcetfiles.Length == 0)
            {
                Log.LogMessage("There are 0 files to process");
                return true;
            }
            Log.LogMessage(MessageImportance.Low, "Starting VCET Modification", null);
            //System.Diagnostics.Debugger.Launch();
            try
            {
                foreach (string vcetFile in vcetfiles)
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(vcetFile);
                        XmlNodeList nodes = null;
                        if (xmlTag.Length != 0)
                        {

                            // here we are also searching on an attribute within the node with XPath,
                            // but not necessarily an attribute we are modifying
                            nodes = doc.SelectNodes(String.Format("//{0}", xmlTag));
                        }

                        if (nodes.Count > 0)
                        {
                            if (attribute.Length != 0)
                            {
                                foreach (XmlNode node in nodes)
                                {
                                    if (node.Attributes[attribute] != null)
                                    {
                                        string typeasstring = node.Attributes[attribute].Value;
                                        string assyname = typeasstring.Split(',')[1].Trim();
                                        string typefromAssy = typeasstring.Split(',')[0].Trim();
                                        string assypath = System.IO.Path.Combine(referencePath, assyname + ".dll");
                                        if (System.IO.File.Exists(assypath))
                                        {
                                            System.Reflection.AssemblyName assy = System.Reflection.AssemblyName.GetAssemblyName(assypath);
                                            string version = assy.Version.ToString();
                                            string pkt = assy.GetPublicKeyToken().ToString();
                                            string newinterface = string.Format("{0}, {1}", new string[] { typefromAssy, assy.FullName });
                                            Log.LogMessage(MessageImportance.High, "Interface is now " + newinterface);
                                            node.Attributes[attribute].Value = newinterface;
                                            doc.Save(vcetFile);
                                        }
                                        else
                                        {
                                            Log.LogError("Could not locate file " + assypath);
                                            return false;
                                        }
                                    }
                                }
                                return true;
                            }
                            Log.LogError("Couldn't find Xml Node - {0}", xmlTag);
                            return false;
                        }
                    }
                    catch (Exception ex2)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }
            return false;
        }
    }
}
