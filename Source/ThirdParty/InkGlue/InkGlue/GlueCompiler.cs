using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Ink.Runtime;
using Ink;

namespace InkGlue
{
	public class GlueInkCompiler: Ink.IFileHandler
	{
		public string ResolveInkFilename(string includeName)
		{
			var workingDir = Directory.GetCurrentDirectory();
			var fullRootInkPath = Path.Combine(workingDir, includeName);
			return fullRootInkPath;
		}

		public string LoadInkFileContents(string fullFilename)
		{
			return File.ReadAllText(fullFilename);
		}

		public GlueInkCompiler(string storyFileContents, string storyFileName)
		{
            errors = new List<string>();

			// Create compiler object
			compiler = new Compiler(storyFileContents, new Compiler.Options
			{
				sourceFilename = storyFileName,
				pluginNames = null,
				countAllVisits = false,
				errorHandler = OnError,
				fileHandler = this
			});

			story = compiler.Compile();
		}

        

        private void OnError(string msg, ErrorType type)
        {
            errors.Add(string.Format("{0}: {1}", msg, type));
        }

		public string CompileToJson()
		{
			return story.ToJsonString();
		}

        public string[] GetErrors()
        {
            return errors.ToArray();
        }

		Story story;
		Compiler compiler;
        List<string> errors;
	}
}
