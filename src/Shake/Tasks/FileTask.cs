//
// Shake - C# Make
//
// File task
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
using System;
using System.IO;
using System.Linq;
using Shake.Interfaces;

namespace Shake.Tasks
{
    /// <summary>
    /// File task.
    /// </summary>
    public class FileTask : IShakeTask
    {
        public static void RemoveDir(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void ReCreateDir(string path)
        {
            RemoveDir(path);
            CreateDir(path);
        }

        /// <summary>
        /// Recursively copies files and folders.
        /// </summary>
        /// <param name="sourcePath">The source directory.</param>
        /// <param name="destPath">The target directory.</param>
        public static void CopyDir(string sourcePath, string destPath, string[] excludePatterns = null, string[] includePatterns = null)
        {
            // skip empty dirs by default (later we can introduce a property)
            if (DirectoryIsEmpty(sourcePath, excludePatterns, includePatterns))
                return;

            Console.WriteLine("Copying \"{0}\" to \"{1}\"", sourcePath, destPath);

            CreateDir(destPath); // create if not exists

            var sourceFiles = GetMathcingFiles(sourcePath, excludePatterns, includePatterns);

            foreach (var sourceFile in sourceFiles)
            {
                var fileName = Path.GetFileName(sourceFile);
                var destFile = Path.Combine(destPath, fileName);
                // later if Verbosity.Full for example, we can log file copying here
                // Console.WriteLine("Copying \"{0}\" to \"{1}\"", sourceFile, destFile);
                File.Copy(sourceFile, destFile, true);
            }

            var sourceDirs = GetMathcingDirs(sourcePath, excludePatterns, includePatterns);

            foreach (var sourceDir in sourceDirs)
            {
                var destDir = Path.Combine(destPath, Path.GetFileName(sourceDir));
                CopyDir(sourceDir, destDir, excludePatterns, includePatterns);
            }
        }

        private static bool DirectoryIsEmpty(string path, string[] excludePatterns, string[] includePatterns)
        {
            var mathcingFiles = GetMathcingFiles(path, excludePatterns, includePatterns);
            var mathcingDirs = GetMathcingDirs(path, excludePatterns, includePatterns);

            return (mathcingFiles.Length == 0 && mathcingDirs.Length == 0);
        }

        private static string[] GetMathcingFiles(string path, string[] excludePatterns, string[] includePatterns)
        {
            if (includePatterns == null)
                includePatterns = new string[] { "*.*" };

            var mathcingFiles = from f in Directory.GetFiles(path)
                                where Path.GetFileName(f).MatchAny(includePatterns)
                                && Path.GetFileName(f).NotMatchAny(excludePatterns)
                                select f;

            return mathcingFiles.ToArray();
        }

        private static string[] GetMathcingDirs(string path, string[] excludePatterns, string[] includePatterns)
        {
            if (includePatterns == null)
                includePatterns = new string[] { "*" };

            var mathcingDirs = from d in Directory.GetDirectories(path)
                               where Path.GetFileName(d).MatchAny(includePatterns)
                               && Path.GetFileName(d).NotMatchAny(excludePatterns)
                               select d;

            return mathcingDirs.ToArray();
        }
    }
}