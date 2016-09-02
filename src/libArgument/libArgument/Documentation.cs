using System;
using System.Reflection;
using System.Text;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Documentation.
    /// </summary>
    public class Documentation<TClass> where TClass: class, new() {

        /// <summary>
        /// The version.
        /// </summary>
        public string Version;

        /// <summary>
        /// The prolog.
        /// </summary>
        public string Prolog;

        /// <summary>
        /// The epilog.
        /// </summary>
        public string Epilog;

        /// <summary>
        /// The description.
        /// </summary>
        public string Description;

        /// <summary>
        /// Prints the documentation.
        /// </summary>
        public void Print () {
            var assembly = Assembly.GetEntryAssembly ();
            var accum = new StringBuilder ();
            var name = assembly.GetName ().Name;
            var version = Version ?? assembly.GetName ().Version.ToString ();
            accum.AppendLine ($"{name} (v{version} {Environment.OSVersion.ToString ()})");
            if (!string.IsNullOrEmpty (Prolog)) {
                accum.AppendLine (Prolog);
            }
            if (!string.IsNullOrEmpty (Description)) {
                accum.AppendLine (Description);
            }
            if (!string.IsNullOrEmpty (Epilog)) {
                accum.AppendLine (Epilog);
            }
            Console.WriteLine (accum);
        }

        /// <summary>
        /// Parses the documentation.
        /// </summary>
        public static Documentation<TClass> Parse () {
            var target = new TClass ();
            var documentation = new Documentation<TClass> ();
            var attributes = target.GetType().GetCustomAttributes (true);
            foreach (var attr in attributes) {
                var doc = attr as DocBase;
                if (doc == null) {
                    continue;
                }
                if (doc is DocVersion) {
                    documentation.Version = doc.Content;
                } else if (doc is DocProlog) {
                    documentation.Prolog = doc.Content;
                } else if (doc is DocEpilog) {
                    documentation.Epilog = doc.Content;
                }
            }
            var fields = target.GetType ().GetFields (
                BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.FlattenHierarchy
            );
            foreach (var field in fields) {
                attributes = field.GetCustomAttributes (true);
                foreach (var attr in attributes) {
                    var doc = attr as Doc;
                    if (doc == null) {
                        continue;
                    }
                    documentation.Description = doc.Content;
                }
            }
            return documentation;
        }
    }
}

