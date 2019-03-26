using System;
using System.Collections.Generic;
using System.Text;

namespace StaticFileTransform.Jsx
{
    public class JsxParser : IJsxParser
    {
        public string ParseAndCompileToJs(string inputJsx, JsxParserOptions options = null)
        {
            var input = inputJsx;
            var output = new StringBuilder();
            var position = 0;
            while(position < input.Length){
                ParseAt(input, ref position, output, options);
            }
            return output.ToString();
        }

        private void ParseAt(string input, ref int position, StringBuilder output, JsxParserOptions options)
        {
            char current = input[position];
            var posBeforeTryParse = position;
            if (current == '<' && TryParseJsx(ref input, ref position, ref output, out var model)){
                GenerateScript(model, output, options);
            }
            else
            {
                position = posBeforeTryParse;
                output.Append(current);
                position++;
            }
        }
        
        private class JsxNode
        {
            internal string Text;
            internal string TagName;

            internal List<Tuple<String, String>> Properties;
            internal List<JsxNode> Children;

            internal bool IsTextNode => Text != null && TagName == null;
            internal bool HasProperties => Properties != null && Properties.Count > 0;
            internal bool HasChildren => Children != null && Children.Count > 0;

            internal void AppendChild(String text) => AppendChild(new JsxNode { Text = text });
            internal void AppendChild(JsxNode node) => UsingChildren().Add(node);
            internal void AppendProperty(string key, string value) => UsingProperties().Add(Tuple.Create(key, value));
            private List<JsxNode> UsingChildren() => Children == null ? Children = new List<JsxNode>() : Children;
            private List<Tuple<String, String>> UsingProperties() => Properties == null ? Properties = new List<Tuple<String, String>>() : Properties;
        }

        private bool TryParseJsx(ref string input, ref int position, ref StringBuilder output, out JsxNode node)
        {
            if (input[position] != '<'){
                node = null;
                return false;
            }
            position++;
            var tagNameStart = position;
            if(!char.IsLetter(input[position])){
                node = null;
                return false;
            }
            while(char.IsLetterOrDigit(input[position])){
                position++;
            }
            node = new JsxNode();
            node.TagName = input.Substring(tagNameStart, position-tagNameStart);

            // there must be a whitespace after the tag name
            {
                var afterTagName = input[position];
                if (afterTagName != '/' && afterTagName != '>' && !char.IsWhiteSpace(afterTagName)){
                    return false;
                }
            }

            // parse attributes
            while (true){
                while(char.IsWhiteSpace(input[position])){
                    position++;
                }
                if (input[position] == '>' || input[position] == '/'){
                    break;
                }
                // attribute keys must start with letter
                if (!char.IsLetter(input[position])){
                    return false;
                }
                var keyStart = position;
                while(char.IsLetterOrDigit(input[position])){
                    position++;
                }
                var key = input.Substring(keyStart, position - keyStart);
                Console.WriteLine(key);
                if (input[position]!='='){
                    node.AppendProperty(key, "true");
                    continue;
                }
                position++;
                var strChar = input[position];
                var valueStart = position;
                var trimFromEnd = 0;
                if (strChar == '{') {
                    strChar = '}';
                    valueStart++; // unwrap
                    trimFromEnd =1;
                }
                position++;
                while(input[position]!=strChar){
                    position++;
                }
                position++;
                var value = input.Substring(valueStart, position - valueStart - trimFromEnd);
                node.AppendProperty(key, value);
            }

            if (input[position] != '>'){
                if(input[position] == '/' && input[position + 1] == '>'){
                    position+=2;
                    return true;
                }
                return false;
            }
            position++;

            while (true){

                // in body of tag
                var bodyStartsAt = position;
                while(input[position] != '<' && input[position] != '{'){
                    position++;
                }
                // end of body
                var bodyLength = position - bodyStartsAt;
                if (bodyLength > 0){
                    var body = input.Substring(bodyStartsAt, position - bodyStartsAt);
                    node.AppendChild($"'{body}'");
                }
                // dynamic section
                if (input[position] == '{'){
                    position++;
                    var dynamicBegin = position;
                    while(input[position] != '}'){
                        position++;
                    }
                    var dynamic = input.Substring(dynamicBegin, position-dynamicBegin);
                    node.AppendChild(dynamic);
                    position++;
                    continue;
                }
                // child tag
                var positionBeforeParse = position;
                if (input[position+1] == '/'){
                    break;
                }
                else if (TryParseJsx(ref input, ref position, ref output, out var child)){
                    node.AppendChild(child);
                }
                else {
                    throw new ArgumentException("Invalid JSX child");
                }
            }


            // check closing tag type
            if (input[position+1] != '/'){
                return false;
            }
            position+=2;
            // closing tag
            var closingTagNameStart = position;

            while(char.IsLetterOrDigit(input[position])){
                position++;
            }
            var closingTag = input.Substring(closingTagNameStart, position - closingTagNameStart);
            if (closingTag != node.TagName){
                throw new ArgumentException("Closinng tag mismatch!");
            }
            if (input[position] != '>'){
                return false;
            }
            position++;
            return true;
        }
        private void GenerateScript(JsxNode model, StringBuilder output, JsxParserOptions options)
        {
            if (model.IsTextNode){
                output.Append(model.Text);
                return;
            }
            output.Append(options.JsCreateElementMethodName);
            output.Append("('");
            output.Append(model.TagName);
            output.Append("'");

            if (model.HasProperties){
                output.Append(',');
                char sep = '{';
                foreach(var (key, value) in model.Properties){
                    output.Append(sep).Append(key).Append(':').Append(value);
                    sep = ',';
                }
                output.Append('}');
            }
            else if (model.HasChildren){
                output.Append(",null");
            }

            if (model.HasChildren){
                foreach(var child in model.Children){
                    output.Append(",");
                    GenerateScript(child, output, options);
                }
            }
            output.Append(")");
        }


    }
}