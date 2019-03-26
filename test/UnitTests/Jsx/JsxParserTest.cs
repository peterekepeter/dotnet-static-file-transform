using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaticFileTransform.Jsx;
using System;

namespace UnitTests.StaticFileTransform.Jsx
{
    [TestClass]
    public class ParserTest
    {
        private readonly IJsxParser parser;
        private readonly JsxParserOptions options;

        public ParserTest()
        {
            options = new JsxParserOptions
            {
                JsCreateElementMethodName = "jsx"
            };
            parser = new JsxParser();
        }

        private String Compile(String input, JsxParserOptions inlineOptions = null) 
            => parser.ParseAndCompileToJs(input, inlineOptions ?? options);

        [TestMethod]
        public void CanParseBasicElementWithTextContent() 
            => Compile("<h1>Hello World!</h1>").Should().Be("jsx('h1',null,'Hello World!')");

        [TestMethod]
        public void CanParseEmptyElement()
            => Compile("<ul></ul>").Should().Be("jsx('ul')");

        [TestMethod]
        public void UsesRenderingFunctionFromOptions()
            => Compile("<ul></ul>", new JsxParserOptions { JsCreateElementMethodName = "render" }).Should().Be("render('ul')");

        [TestMethod]
        public void CanParseSelfClosingElement()
            => Compile("<br/>").Should().Be("jsx('br')");

        [TestMethod]
        public void CanParseProperties()
            => Compile("<br className='dark'/>").Should().Be("jsx('br',{className:'dark'})");

        [TestMethod]
        public void CanParseMultipleProperties()
            => Compile("<br className='dark' height='16px'/>").Should().Be("jsx('br',{className:'dark',height:'16px'})");

        [TestMethod]
        public void CanParseChildren()
            => Compile("<ul><li>hi!</li></ul>").Should().Be("jsx('ul',null,jsx('li',null,'hi!'))");

        [TestMethod]
        public void CanParseMultipleChildren()
            => Compile("<ul><li>hi!</li><li>hello!</li></ul>")
            .Should().Be("jsx('ul',null,jsx('li',null,'hi!'),jsx('li',null,'hello!'))");

        [TestMethod]
        public void CanParseChildrenOfChildren()
            => Compile("<div><p><ul><li>hi!</li></ul></p></div>")
                .Should().Be("jsx('div',null,jsx('p',null,jsx('ul',null,jsx('li',null,'hi!'))))");
        
        [TestMethod]
        public void CanParseDynamicContent()
            => Compile("<h2>{state.message}</h2>")
                .Should().Be("jsx('h2',null,state.message)");

        [TestMethod]
        public void CanParseDynamicContentWithStaticContent()
            => Compile("<h2>Hello {state.name} how are you?</h2>")
                .Should().Be("jsx('h2',null,'Hello ',state.name,' how are you?')");

        [TestMethod]
        public void CanParseElementWithTextAndElementContent()
            => Compile("<h2>Hello <strong>{state.name}</strong> how are you?</h2>")
                .Should().Be("jsx('h2',null,'Hello ',jsx('strong',null,state.name),' how are you?')");

        [TestMethod]
        public void CanParseElementWithDynamicAttribute()
            => Compile("<div className={state.style}>...content</div>")
            .Should().Be("jsx('div',{className:state.style},'...content')");

        [TestMethod]
        public void CanParseElementWithDynamicAttributeWhichIsAFuncction()
            => Compile("<button onclick={event=>console.log(event)}>Click Me!</button>")
                .Should().Be("jsx('button',{onclick:event=>console.log(event)},'Click Me!')");

        [TestMethod]
        [DataRow("if (x<0) console.log('x is less than 0');")]
        [DataRow("function square(x) { return x*x; }")]
        [DataRow("function factorial(x) { var y=1; for(var i=0; i<x; i++) y*=x; return y;}")]
        [DataRow("function print(x,m) { while(x>0) { x=x-1; console.log(m); }; }")]
        [DataRow("while(x<y && y<z) { something() }")]
        public void DoesNotAffectRegularJavascript(String javascript)
            => Compile(javascript).Should().Be(javascript);

        [TestMethod]
        public void FunctionalRenderingTestWithClassingFunctionType()
            => Compile("function render(name){ return <h1>Hello {name}</h1>; }")
            .Should().Be("function render(name){ return jsx('h1',null,'Hello ',name); }");

        [TestMethod]
        public void FunctionalRenderingTestWithArrowFunctionType()
            => Compile("var userRow = user => <tr><td>{user.name}</td><td>{user.email}</td></tr>;")
            .Should().Be("var userRow = user => jsx('tr',null,jsx('td',null,user.name),jsx('td',null,user.email));");
        
        [TestMethod]
        public void AttriubtesWithoutValueAreBoolean()
            => Compile("<button disabled/>")
            .Should().Be("jsx('button',{disabled:true})");

    }
}
