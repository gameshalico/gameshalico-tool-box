using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

namespace AutoReferenceGenerator.Editor
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

        private static readonly string[] s_invalidCharacters =
        {
            " ", "!", "\"", "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "-", ".", "/", ":", ";", "<", "=", ">",
            "?", "@", "[", "\\", "]", "^", "_", "`", "{", "|", "}", "~"
        };

        private static readonly string s_indent = "    ";
        private readonly List<Field> _fields = new();
        private string _className;


        public string Namespace { get; set; }

        public AccessModifier ClassAccessModifier { get; set; }

        public string ClassName
        {
            get => _className;
            set => _className = Sanitize(value);
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

        private static string Sanitize(string identifier)
        {
            foreach (var invalidCharacter in s_invalidCharacters) identifier = identifier.Replace(invalidCharacter, "");

            return identifier;
        }

        private static string Indent(int count)
        {
            var indent = "";
            for (var i = 0; i < count; i++) indent += s_indent;

            return indent;
        }

        public void AddConstantField(string type, string identifier, string value,
            AccessModifier accessModifier = AccessModifier.Public)
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
            builder.AppendLine($"namespace {Namespace}");

            builder.AppendLine("{");
            builder.AppendLine(Indent(1) + $"public class {_className}");
            builder.AppendLine(Indent(1) + "{");
            foreach (var field in _fields) builder.AppendLine(Indent(2) + field);
            builder.AppendLine(Indent(1) + "}");
            builder.AppendLine("}");

            return builder.ToString();
        }

        public void Generate(string directoryPath)
        {
            var path = $"{directoryPath}/{_className}.cs";

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            File.WriteAllText(path, GenerateScript(), Encoding.UTF8);
            AssetDatabase.Refresh();
        }

        public class Member
        {
            public string Identifier { get; set; }
            public AccessModifier AccessModifier { get; set; }
        }

        private class Field : Member
        {
            public string Type { get; set; }
            public string Value { get; set; }
            public bool IsStatic { get; }
            public bool IsConstant { get; set; }
            public bool IsReadonly { get; }

            public override string ToString()
            {
                var line = AccessModifierToString(AccessModifier);
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
    }
}