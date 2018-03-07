<%@ Page Language="C#" Async="true" %>

<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="Shinobytes.XzaarScript.VM" %>
<%@ Import Namespace="Shinobytes.XzaarScript.Assembly.Models" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    public string ConsoleOutput;
    public string CurrentSourceCode = @"// variable localNames starting with $ 
// are variables grabbed from an external source
let console = $console

fn print_hello_world() -> void {
   console.log(""Hello World"")
}

print_hello_world()";
    private async void OnClick(object sender, EventArgs e)
    {
        CurrentSourceCode = this.Request.Form["xzaarscript"];

        var console = new ConsoleWrapper();
        try
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var rt = new Shinobytes.XzaarScript.ScriptInterpreter(CurrentSourceCode);
            rt.RegisterVariable("$console", console);
            sw.Stop();

            if (rt.HasErrors)
                throw new Exception(string.Join("<br/>", rt.Errors.Select(x => "[" + x.ErrorLocation + "] " + x.Error)));

            var loadTime = sw.ElapsedMilliseconds;

            sw.Restart();

            // rt.Invoke<object>("main", console);
            rt.Run();
            sw.Stop();
            this.ConsoleOutput = "<span style='color: #999'>[Code executed in " + (loadTime + sw.ElapsedMilliseconds) + "ms (Load: " + loadTime + "ms, Exec: " + sw.ElapsedMilliseconds + ")]</span><br/>";
            this.ConsoleOutput += console.Output;
        }
        catch (Exception exc)
        {
            this.ConsoleOutput += "<span style='color: red;'>" + exc.Message + "</span>";
        }

        //try
        //{
        //    var rt = Shinobytes.XzaarScript.WebInterpreter.Test_Helper_Extensions.Run(CurrentSourceCode);
        //    rt.Invoke<object>("main", console);
        //    this.ConsoleOutput += console.Output;
        //}
        //catch (Exception exc)
        //{
        //    this.ConsoleOutput += exc.Message;
        //}
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
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" integrity="sha384-wvfXpqpZZVQGK6TAh5PVlGOfQNHSoD2xbE+QkPxCAFlNEevoEH3Sl0sibVcOQVnN" crossorigin="anonymous">
    <style type="text/css" media="screen">
        #editor { position: absolute; top: 50px; right: 0; bottom: 30%; left: 0; }
        #console { position: absolute; top: 70%; right: 0; bottom: 0; left: 0; border-top: 3px solid #ccc; background-color: #222; color: white; }
        .control-panel { position: absolute; top: 0; right: 0; left: 0; height: 50px; background-color: #2b6d98; color: white; border-bottom: 1px solid #1d4864; }
        .compile-button:hover { background-color: rgba(0,0,0,0.1); }
        .compile-button { color: white; text-decoration: none; /* margin-left: 15px; */ top: 1px; position: absolute; padding: 15px; }
    </style>
</head>
<body>
    <form id="HtmlForm" runat="server">
        <div class="control-panel">
            <asp:HiddenField runat="server" ID="xzaarscript" ClientIDMode="Predictable" />
            <asp:LinkButton runat="server" CssClass="compile-button" OnClick="OnClick" OnClientClick="var onCompileAndRunClicked = function() { document.querySelector('#xzaarscript').value = editor.getValue(); }; onCompileAndRunClicked();">
                <span class="fa fa-play-circle"></span>&nbsp;Run
            </asp:LinkButton>
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
