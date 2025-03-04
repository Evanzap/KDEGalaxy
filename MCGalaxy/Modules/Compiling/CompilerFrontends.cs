/*
    Copyright 2010 MCLawl Team - Written by Valek (Modified by MCGalaxy)

    Edited for use with MCGalaxy
 
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
#if !MCG_STANDALONE
using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;

namespace MCGalaxy.Modules.Compiling
{
    public sealed class CSCompiler : ICompiler 
    {
        public override string FileExtension { get { return ".cs"; } }
        public override string ShortName     { get { return "C#"; } }  
        public override string FullName      { get { return "CSharp"; } }

#if !NETSTANDARD
        CodeDomProvider compiler;

        protected override ICompilerErrors DoCompile(string[] srcPaths, string dstPath) {
            CompilerParameters args = ICodeDomCompiler.PrepareInput(srcPaths, dstPath, "//");
            args.CompilerOptions   += " /unsafe";
            // NOTE: Make sure to keep CompilerOptions in sync with RoslynCSharpCompiler

            ICodeDomCompiler.InitCompiler(this, "CSharp", ref compiler);
            return ICodeDomCompiler.Compile(args, srcPaths, compiler);
        }
#else
        protected override ICompilerErrors DoCompile(string[] srcPaths, string dstPath) {
            List<string> referenced = ProcessInput(srcPaths, "//");
            return RoslynCSharpCompiler.Compile(srcPaths, dstPath, referenced);
        }
#endif

        public override string CommandSkeleton {
            get {
                return @"//\tAuto-generated command skeleton class
//\tUse this as a basis for custom MCGalaxy commands
//\tNaming should be kept consistent (e.g. /update command should have a class name of 'CmdUpdate' and a filename of 'CmdUpdate.cs')
// As a note, MCGalaxy is designed for .NET 4.0

// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""

// Add any other using statements you need after this
using System;
using MCGalaxy;

public class Cmd{0} : Command
{{
\t// The command's name (what you put after a slash to use this command)
\tpublic override string name {{ get {{ return ""{0}""; }} }}

\t// Command's shortcut, can be left blank (e.g. ""/Copy"" has a shortcut of ""c"")
\tpublic override string shortcut {{ get {{ return """"; }} }}

\t// Which submenu this command displays in under /Help
\tpublic override string type {{ get {{ return ""other""; }} }}

\t// Whether or not this command can be used in a museum. Block/map altering commands should return false to avoid errors.
\tpublic override bool museumUsable {{ get {{ return true; }} }}

\t// The default rank required to use this command. Valid values are:
\t//   LevelPermission.Guest, LevelPermission.Builder, LevelPermission.AdvBuilder,
\t//   LevelPermission.Operator, LevelPermission.Admin, LevelPermission.Owner
\tpublic override LevelPermission defaultRank {{ get {{ return LevelPermission.Guest; }} }}

\t// This is for when a player executes this command by doing /{0}
\t//   p is the player object for the player executing the command. 
\t//   message is the arguments given to the command. (e.g. for '/{0} this', message is ""this"")
\tpublic override void Use(Player p, string message)
\t{{
\t\tp.Message(""Hello World!"");
\t}}

\t// This is for when a player does /Help {0}
\tpublic override void Help(Player p)
\t{{
\t\tp.Message(""/{0} - Does stuff. Example command."");
\t}}
}}";
            }
        }
        
        public override string PluginSkeleton {
            get {
                return @"//\tAuto-generated plugin skeleton class
//\tUse this as a basis for custom MCGalaxy plugins

// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""

// Add any other using statements you need after this
using System;

namespace MCGalaxy
{{
\tpublic class {0} : Plugin
\t{{
\t\t// The plugin's name (i.e what shows in /Plugins)
\t\tpublic override string name {{ get {{ return ""{0}""; }} }}

\t\t// The oldest version of MCGalaxy this plugin is compatible with
\t\tpublic override string MCGalaxy_Version {{ get {{ return ""{2}""; }} }}

\t\t// Message displayed in server logs when this plugin is loaded
\t\tpublic override string welcome {{ get {{ return ""Loaded Message!""; }} }}

\t\t// Who created/authored this plugin
\t\tpublic override string creator {{ get {{ return ""{1}""; }} }}

\t\t// Called when this plugin is being loaded (e.g. on server startup)
\t\tpublic override void Load(bool startup)
\t\t{{
\t\t\t//code to hook into events, load state/resources etc goes here
\t\t}}

\t\t// Called when this plugin is being unloaded (e.g. on server shutdown)
\t\tpublic override void Unload(bool shutdown)
\t\t{{
\t\t\t//code to unhook from events, dispose of state/resources etc goes here
\t\t}}

\t\t// Displays help for or information about this plugin
\t\tpublic override void Help(Player p)
\t\t{{
\t\t\tp.Message(""No help is available for this plugin."");
\t\t}}
\t}}
}}";
            }
        }
    }
    
    public sealed class VBCompiler : ICompiler 
    {
        public override string FileExtension { get { return ".vb"; } }
        public override string ShortName     { get { return "VB"; } }
        public override string FullName      { get { return "Visual Basic"; } }
        
#if !NETSTANDARD
        CodeDomProvider compiler;

        protected override ICompilerErrors DoCompile(string[] srcPaths, string dstPath) {
            CompilerParameters args = ICodeDomCompiler.PrepareInput(srcPaths, dstPath, "'");

            ICodeDomCompiler.InitCompiler(this, "VisualBasic", ref compiler);
            return ICodeDomCompiler.Compile(args, srcPaths, compiler);
        }
#else
        protected override ICompilerErrors DoCompile(string[] srcPaths, string dstPath) {
            throw new NotSupportedException("Compiling Visual Basic is not supported in .NET Standard build");
        }
#endif
        
        public override string CommandSkeleton {
            get {
                return @"'\tAuto-generated command skeleton class.
'\tUse this as a basis for custom MCGalaxy commands.
'\tNaming should be kept consistent. (e.g. /update command should have a class name of 'CmdUpdate' and a filename of 'CmdUpdate.vb')
' As a note, MCGalaxy is designed for .NET 4.0.

' To reference other assemblies, put a ""'reference [assembly filename]"" at the top of the file
'   e.g. to reference the System.Data assembly, put ""'reference System.Data.dll""

' Add any other Imports statements you need after this
Imports System
Imports MCGalaxy

Public Class Cmd{0}
\tInherits Command

\t' The command's name (what you put after a slash to use this command)
\tPublic Overrides ReadOnly Property name() As String
\t\tGet
\t\t\tReturn ""{0}""
\t\tEnd Get
\tEnd Property

\t' Command's shortcut, can be left blank (e.g. ""/Copy"" has a shortcut of ""c"")
\tPublic Overrides ReadOnly Property shortcut() As String
\t\tGet
\t\t\tReturn """"
\t\tEnd Get
\tEnd Property

\t' Which submenu this command displays in under /Help   
\tPublic Overrides ReadOnly Property type() As String
\t\tGet
\t\t\tReturn ""other""
\t\tEnd Get
\t End Property

\t' Whether or not this command can be used in a museum. Block/map altering commands should return False to avoid errors.
\tPublic Overrides ReadOnly Property museumUsable() As Boolean
\t\tGet
\t\t\tReturn True
\t\tEnd Get
\tEnd Property

\t' The default rank required to use this command. Valid values are:
\t'   LevelPermission.Guest, LevelPermission.Builder, LevelPermission.AdvBuilder,
\t'   LevelPermission.Operator, LevelPermission.Admin, LevelPermission.Owner
\tPublic Overrides ReadOnly Property defaultRank() As LevelPermission
\t\tGet
\t\t\tReturn LevelPermission.Guest
\t\tEnd Get
\tEnd Property

\t' This is for when a player executes this command by doing /{0}
\t'   p is the player object for the player executing the command.
\t'   message is the arguments given to the command. (e.g. for '/{0} this', message is ""this"")
\tPublic Overrides Sub Use(p As Player, message As String)
\t\tp.Message(""Hello World!"")
\tEnd Sub

\t' This is for when a player does /Help {0}
\tPublic Overrides Sub Help(p As Player)
\t\tp.Message(""/{0} - Does stuff. Example command."")
\tEnd Sub
End Class";
            }
        }
        
        public override string PluginSkeleton {
            get {
                return @"' This is an example plugin source!
Imports System

Namespace MCGalaxy
\tPublic Class {0}
\t\tInherits Plugin

\t\tPublic Overrides ReadOnly Property name() As String
\t\t\tGet
\t\t\t\tReturn ""{0}""
\t\t\tEnd Get
\t\t End Property
\t\tPublic Overrides ReadOnly Property MCGalaxy_Version() As String
\t\t\tGet
\t\t\t\tReturn ""{2}""
\t\t\tEnd Get
\t\t End Property
\t\tPublic Overrides ReadOnly Property welcome() As String
\t\t\tGet
\t\t\t\tReturn ""Loaded Message!""
\t\t\tEnd Get
\t\t End Property
\t\tPublic Overrides ReadOnly Property creator() As String
\t\t\tGet
\t\t\t\tReturn ""{1}""
\t\t\tEnd Get
\t\t End Property

\t\tPublic Overrides Sub Load(startup As Boolean)
\t\t\t' LOAD YOUR PLUGIN WITH EVENTS OR OTHER THINGS!
\t\tEnd Sub
                        
\t\tPublic Overrides Sub Unload(shutdown As Boolean)
\t\t\t' UNLOAD YOUR PLUGIN BY SAVING FILES OR DISPOSING OBJECTS!
\t\tEnd Sub
                        
\t\tPublic Overrides Sub Help(p As Player)
\t\t\t' HELP INFO!
\t\tEnd Sub
\tEnd Class
End Namespace";
            }
        }
    }    
}
#endif