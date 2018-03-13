<%@ Page Language="C#" Async="true" %>

<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="Shinobytes.XzaarScript.VM" %>
<%@ Import Namespace="Shinobytes.XzaarScript.Assembly" %>
<%@ Import Namespace="Shinobytes.XzaarScript.Extensions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    public string ConsoleOutput;
    public string CurrentSourceCode = @"// variable names starting with $ 
// are variables grabbed from an external source
let console = $console

fn print_hello_world() -> void {
   console.log(""Hello World"")
}

print_hello_world()";

    private Shinobytes.XzaarScript.VM.Runtime Load(string inputCode)
    {
        var compileExpression = Shinobytes.XzaarScript.Extensions.ScriptVmExtensions.CompileExpression(
            Shinobytes.XzaarScript.Extensions.ScriptVmExtensions.Parse(
                Shinobytes.XzaarScript.Extensions.ScriptVmExtensions.Tokenize(inputCode)));        
        var asm = Shinobytes.XzaarScript.Extensions.ScriptVmExtensions.Compile(compileExpression);                
        return  Shinobytes.XzaarScript.Extensions.ScriptVmExtensions.Load(asm);
    }

    private async void OnClick(object sender, EventArgs e)
    {
        CurrentSourceCode = this.Request.Form["xzaarscript"];

        var console = new ConsoleWrapper();
        try
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            var rt = Load(CurrentSourceCode);
            rt.RegisterGlobalVariable("console", console);

            sw.Stop();

            var loadTime = sw.ElapsedMilliseconds;

            // if (rt.HasErrors)
            // {
            //     this.ConsoleOutput = "<span style='color: #999'>[Code executed in " + (loadTime + sw.ElapsedMilliseconds) + "ms (Load: " + loadTime + "ms, Exec: " + sw.ElapsedMilliseconds + ")]</span><br/>";
            //     this.ConsoleOutput += "<span style='color: red;'>" + string.Join("<br/>", rt.Errors.Select(x => "[" + x.ErrorLocation + "] " + x.Error)) + "</span>";                
            //     return;
            // }

            sw.Restart();
            rt.Run();
            sw.Stop();

            this.ConsoleOutput = "<span style='color: #999'>[Code executed in " + (loadTime + sw.ElapsedMilliseconds) + "ms (Load: " + loadTime + "ms, Exec: " + sw.ElapsedMilliseconds + ")]</span><br/>";
            this.ConsoleOutput += console.Output;
        }
        catch (Exception exc)
        {
            this.ConsoleOutput += "<span style='color: red;'>" + exc.Message + "</span>";
        }
    }

    public class ConsoleWrapper
    {
        public string Output;

        public void log(object text)
        {
            if (text is RuntimeVariable)
            {
                Output += ((RuntimeVariable)text).Value + "<br/>";
            }
            else if (text is Constant)
            {
                Output += ((Constant)text).Value + "<br/>";
            }
            else
            {
                Output += text + "<br/>";
            }
        }
        private string SecretMethod2()
        {
            return "wtf!";
        }
        public string SecretMethod()
        {
            return "Shh! It was a secret!";
        }
    }

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Title</title>
    <link href="https://fonts.googleapis.com/css?family=Amatic+SC" rel="stylesheet">
    <script defer src="https://use.fontawesome.com/releases/v5.0.8/js/all.js"></script>
    <style type="text/css" media="screen">
        body {
			background-color: #222;
            font-family: -apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,Oxygen,Ubuntu,Cantarell,"Fira Sans","Droid Sans","Helvetica Neue",Arial,sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol";
        }

        .logo {
            font-family: 'Amatic SC', cursive;
            position: relative;
            font-size: 18pt;
            left: 18px;
            top: 8px;
        }

        .logo a {
            text-decoration: none;
            color: white;
        }

        #editor {
            position: absolute;
            top: 50px;
            right: 0;
            bottom: 30%;
            left: 0;
        }

        #console {
            position: absolute;
            top: 70%;
            right: 0;            
            left: 0;
            border-top: 1px solid #2f3129;
            background-color: #222;
            color: white;
            padding: 10px;
        }

        .control-panel {
            position: absolute;
            top: 0;
            right: 0;
            left: 0;
            height: 50px;
            background-color: #24251f;
            color: white;
            border-bottom: 1px solid #1d4864;
            font-size: 12px;
            width: 100%;
            overflow: hidden;
        }

        .compile-button:hover {
            background-color: rgba(0,0,0,0.1);
            border-bottom: 1px solid red;
        }

        .compile-button {
            color: white;
            text-decoration: none;
            top: -30px;
            position: relative;
            padding: 15px;
            height: 19px;
            display: inline-block;
            left: 110px;
        }
        
        .github-button {
            color: white;
            text-decoration: none;
            top: -30px;
            position: relative;
            padding: 15px;
            height: 19px;
            display: inline-block;
            left: 110px;
        }

        .action-text {
            font-size: 14px;
            padding-left: 11px;
            position: relative;
            top: 1px;
        }
    </style>
</head>
<body>
    <form id="HtmlForm" runat="server">
        <div class="control-panel">
            <div class="logo"><a href="/">XzaarScript</a></div>
            <asp:HiddenField runat="server" ID="xzaarscript" ClientIDMode="Predictable" />
            <asp:LinkButton runat="server" ToolTip="Click to run" CssClass="compile-button" OnClick="OnClick" OnClientClick="var onCompileAndRunClicked = function() { document.querySelector('#xzaarscript').value = editor.getValue(); }; onCompileAndRunClicked();">
                <span class="fa fa-play"></span><span class="action-text">Run</span>
            </asp:LinkButton>
            
            <a title="Visit XzaarScript github page" target="_blank" href="https://github.com/zerratar/XzaarScript"><div class="github-button"><span class="fab fa-github"></span></div></a>
        </div>
        <div id="editor"><%= this.CurrentSourceCode %></div>
        <div id="console">
            <div class="console_output"><%= this.ConsoleOutput %></div>
        </div>
    </form>
    <script src="/ace/ace.js" type="text/javascript" charset="utf-8"></script>
    <script>
        var editor = ace.edit("editor");
        editor.setTheme("ace/theme/monokai");
        editor.getSession().setMode("ace/mode/rust");
    </script>
</body>
</html>
