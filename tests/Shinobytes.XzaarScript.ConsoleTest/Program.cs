/* 
 *  This file is part of XzaarScript.
 *  Copyright © 2018 Karl Patrik Johansson, zerratar@gmail.com
 *
 *  XzaarScript is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  XzaarScript is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with XzaarScript.  If not, see <http://www.gnu.org/licenses/>. 
 *  
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shinobytes.XzaarScript.Extensions;
using Shinobytes.XzaarScript.VM;

namespace Shinobytes.XzaarScript.ConsoleTest
{
    public class testclass
    {
        public object i = null;

        public int x = 999;
        public int y = 100;

        private string str;

        public double j;

        public testclass Compiler_jack()
        {
            this.j = 0.0;
            while (true)
            {
                this.j = this.j + 1.0;
                if (this.j == 10.0)
                {
                    return this;
                }
            }

        }

        public void loop_post_increment_break_when_10()
        {
            this.j = 0;
            do
            {
                this.j++;
                if (this.j == 10)
                    break;
            } while (true);

        }
        public void post_increment_test()
        {
            x = 0;
            y = x++;
        }

        public void more_than()
        {
            var i = x > y;
        }

        public void more_than_equals()
        {
            var i = x >= y;
        }

        public void less_than()
        {
            var i = x < y;
        }


        public void less_than_equals()
        {
            var i = x <= y;
        }

        public void less_than_equals_or_more_than_equals()
        {
            var i = x <= y || x >= y;
        }

        public void less_than_and_more_than()
        {
            var i = x < y && x > y;
        }

        public void not_equals()
        {
            var i = x != y;
        }

        public bool not_equals_2()
        {
            return x != y;
        }

        public void both_equals()
        {
            var i = x == y;
        }

        public bool both_equals_2()
        {
            return x == y;
        }

        public void if_x_equals_y_then_console_writeline()
        {
            if (x == y)
            {
                Console.WriteLine();
            }
        }

        public void if_x_not_equals_y_then_console_writeline()
        {
            if (x != y)
            {
                Console.WriteLine();
            }
        }

        public void if_x_more_than_y_or_y_more_than_x_then_console_writeline()
        {
            if (x > y || y > x)
            {
                Console.WriteLine();
            }
        }


        public void while_true()
        {
            var i = 0;
            while (true)
            {
                i++;
            }
        }

        public void while_true_break()
        {
            var i = 0;
            while (true)
            {
                break;
                i++;
            }
        }

        public void if_x_more_than_y_then_console_writeline()
        {
            if (x > y)
            {
                Console.WriteLine();
            }
        }

        public void if_x_less_than_y_then_console_writeline()
        {
            if (x < y)
            {
                Console.WriteLine();
            }
        }

        public void if_x_more_than_equals_y_then_console_writeline()
        {
            if (x >= y)
            {
                Console.WriteLine();
            }
        }

        public void if_x_less_than_equals_y_then_console_writeline()
        {
            if (x <= y)
            {
                Console.WriteLine();
            }
        }

        public void concat_string_with_integer()
        {
            var a = "hello there, " + 9;
        }

        public void concat_string_with_integer_field()
        {
            str = "hello there, " + 9;
        }

        public void concat_string_with_post_increment_integer()
        {
            var b = 9;
            var a = "hello there, " + b++;
        }

        public void concat_string_with_pre_increment_integer()
        {
            var b = 9;
            var a = "hello there, " + ++b;
        }

        public void concat_string_with_integer_using_additional_variables()
        {
            var a = 9;
            var b = "hello there, ";
            var c = b + a;
        }

        public void concat_string_with_string()
        {
            var b = "world";
            var a = "hello " + b;
        }

        public void concat_string_with_struct()
        {
            var t = new test_struct();
            var a = "hello " + t;
        }

        public void concat_string_with_class()
        {
            var t = new test_class();
            var a = "hello " + t;
        }

        public void concat_string_with_enum()
        {
            var t = test_enum.test_value_1;
            var a = "hello " + t;
        }

        public void take_struct_in_object_parameter()
        {
            var s = new test_struct();
            object_input(s);
        }

        public void take_struct_in_struct_parameter()
        {
            var s = new test_struct();
            struct_input(s);
        }


        public void foreach_loop_list()
        {
            var _is = new List<int> { 1, 2, 3 };
            foreach (var i in _is) Console.WriteLine(i);
        }

        public void foreach_loop_array()
        {
            var _is = new int[] { 1, 2, 3 };
            foreach (var i in _is) Console.WriteLine(i);
        }

        public void for_loop()
        {
            for (var i = 0; i < 9999; i++)
            {
                Console.WriteLine(i);
            }
        }

        public void for_loop_custom()
        {
            for (var i = 0; i < 9999;)
            {
                Console.WriteLine(i);
                i++;
            }
        }

        public void while_loop()
        {
            var isTrue = true;
            while (isTrue)
            {
                isTrue = false;
            }
        }

        public void do_while_loop()
        {
            var i = 0;
            var isTrue = true;
            do
            {
                if (i == 4)
                    isTrue = false;
                i++;
            } while (isTrue);
        }

        private void object_input(object s)
        {
        }

        private void struct_input(test_struct s)
        {
            s.test = "hello world";
        }


        private enum test_enum { test_value_1, test_value_2 }
        private class test_class { }

        private struct test_struct
        {
            public string test;
        }
    }

    class Program
    {



        public string test(string ttt)
        {
            ttt = "hello world";
            return ttt;
        }



        static void Main(string[] args)
        {
            var script = @"let val = 0

fn test(any str) { 
    val = val + 1
}


for(let j = 0; j < 1000000; j++) { 
    test(""hello"") 
}


$console.log(val)";


            var console = new ConsoleWrapper();
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var rt = new Interpreter(script);
                rt.RegisterVariable("$console", console);
                sw.Stop();
                var loadTime = sw.ElapsedMilliseconds;

                sw.Restart();

                // rt.Invoke<object>("main", console);
                rt.Run();
                sw.Stop();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("[Code executed in " + (loadTime + sw.ElapsedMilliseconds) + "ms (Load: " + loadTime + "ms, Exec: " + sw.ElapsedMilliseconds + ")]");
                Console.ResetColor();
                Console.WriteLine(console.Output);
            }
            catch (Exception exc)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exc.Message);
            }
            Console.ReadKey();
        }
    }

    public class ConsoleWrapper
    {
        public string Output;

        public void log(object text)
        {
            var rtVar = text as RuntimeVariable;
            if (rtVar != null)
            {
                Output += rtVar.Value + "";
            }
            else
            {
                Output += text + "";
            }
        }

        public string SecretMethod()
        {
            return "Shh! It was a secret!";
        }
    }
}
