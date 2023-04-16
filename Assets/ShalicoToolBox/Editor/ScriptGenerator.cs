using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Shalico.ToolBox.Editor
{
    public class ScriptGenerator
    {
        public enum AccessModifier
        {
            Public,
            Private,
            Protected,
            Internal,
            ProtectedInternal,
            PrivateProtected
        }
        private static string AccessModifierToString(AccessModifier accessModifier)
        {
            return accessModifier switch
            {
                AccessModifier.Public => "public",
                AccessModifier.Private => "private",
                AccessModifier.Protected => "protected",
                AccessModifier.Internal => "internal",
                AccessModifier.ProtectedInternal => "protected internal",
                AccessModifier.PrivateProtected => "private protected",
                _ => throw new ArgumentOutOfRangeException(nameof(accessModifier), accessModifier, null)
            };
        }

        public class Member
        {
            public string Identifier { get; set; }
            public AccessModifier AccessModifier { get; set; }
        }
        public class Field : Member
        {
            public string Type { get; set; }
            public string Value { get; set; }
            public bool IsStatic { get; set; }
            public bool IsConstant { get; set; }
            public bool IsReadonly { get; set; }

            public override string ToString()
            {
                string line = AccessModifierToString(AccessModifier);
                if (IsStatic)
                    line += " static";
                if (IsConstant)
                    line += " const";
                if (IsReadonly)
                    line += " readonly";

                line += $" {Type} {Identifier}";

                if (Value != null)
                    line += $" = {Value}";
                line += ";";

                return line;
            }
        }

        private static readonly string[] s_invalidCharacters = new string[]
        {
            " ", "!", "\"", "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "-", ".", "/", ":", ";", "<", "=", ">", "?", "@", "[", "\\", "]", "^", "_", "`", "{", "|", "}", "~"
        };
        private static string Sanitize(string identifier)
        {
            foreach (var invalidCharacter in s_invalidCharacters)
            {
                identifier = identifier.Replace(invalidCharacter, "");
            }

            return identifier;
        }
        private static readonly string s_indent = "    ";
        private static string Indent(int count)
        {
            string indent = "";
            for (int i = 0; i < count; i++)
            {
                indent += s_indent;
            }

            return indent;
        }


        private string _namespace;
        private AccessModifier _accessModifier;
        private string _className;
        private List<Field> _fields = new();

        public string Namespace
        {
            get => _namespace;
            set => _namespace = value;
        }
        public AccessModifier ClassAccessModifier
        {
            get => _accessModifier;
            set => _accessModifier = value;
        }
        public string ClassName
        {
            get => _className;
            set => _className = Sanitize(value);
        }

        public void AddConstantField(string type, string identifier, string value, AccessModifier accessModifier = AccessModifier.Public)
        {
            _fields.Add(new Field
            {
                AccessModifier = accessModifier,
                Identifier = Sanitize(identifier),
                IsConstant = true,
                Type = type,
                Value = value
            });
        }

        public string GenerateScript()
        {
            StringBuilder builder = new();
            builder.AppendLine($"namespace {_namespace}");
            
            builder.AppendLine("{");
            builder.AppendLine(Indent(1) + $"public class {_className}");
            builder.AppendLine(Indent(1) + "{");
            foreach (var field in _fields)
            {
                builder.AppendLine(Indent(2) + field.ToString());
            }
            builder.AppendLine(Indent(1) + "}");
            builder.AppendLine("}");

            return builder.ToString();
        }

        public void Generate(string directoryPath)
        {
            string path = $"{directoryPath}/{_className}.cs";

            if(!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            File.WriteAllText(path, GenerateScript(), Encoding.UTF8);
            AssetDatabase.Refresh();
        }
    }
}