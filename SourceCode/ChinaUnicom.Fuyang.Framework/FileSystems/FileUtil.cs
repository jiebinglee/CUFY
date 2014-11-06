/**
 * Project Title: Lilly Work Flow Site
 * Description: Lilly Work Flow Site
 * Copyright: Copyright (C) 2010
 * Company: Lilly China
 *
 */
using System;
using System.IO;

namespace ChinaUnicom.Fuyang.Framework.FileSystems
{
    /// <summary>
    /// Basic method for file.
    /// </summary>
    public sealed class FileUtil
    {
        /// <summary>
        /// Write buffer to file.
        /// </summary>
        /// <param name="strPath">File path</param>
        /// <param name="Buffer">Buffer string</param>
        public static void WriteToFile(string strPath, ref byte[] Buffer)
        {
            // Create a file
            FileStream newFile = new FileStream(strPath, FileMode.Create);

            // Write data to the file
            newFile.Write(Buffer, 0, Buffer.Length);

            // Close file
            newFile.Close();
        }

        /// <summary>
        /// Clear a folder
        /// </summary>
        /// <param name="folderName">folder name</param>
        public static void ClearFolder(string folderName)
        {
            try
            {
                if (Directory.Exists(folderName))
                {
                    Directory.Delete(folderName, true);
                }

                Directory.CreateDirectory(folderName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// delete file
        /// </summary>
        /// <param name="fullPath">File Path</param>
        public static void DeleteFile(string fullPath)
        {
            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// According to the Rscript String to create the R entry file
        /// </summary>   
        public static void CreateFile(string content, string path, string fileName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path + fileName);
                sw.Write(content);
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Copy file from original file to target file
        /// </summary>
        /// <param name="originalFilePath"></param>
        /// <param name="targetFilePath"></param>
        public static void CopyFile(string originalFilePath, string targetFilePath)
        {
            try
            {
                FileStream fs = new FileStream(originalFilePath, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                StreamWriter sw = new StreamWriter(targetFilePath);

                while (!sr.EndOfStream)
                {
                    string lineStr = sr.ReadLine();
                    sw.WriteLine(lineStr);
                }
                fs.Close();
                sr.Close();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Ensure the folder path exists.
        /// </summary>
        /// <param name="path">Folder path</param>
        /// <returns>Return path</returns>
        public static string EnsureDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        [Obsolete("EnsureEndWithSeperatorChar Method instead.")]
        public static string EnsureFolderName(string folderName)
        {
            DirectoryInfo di = null;
            try
            {
                di = new DirectoryInfo(folderName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string path = di.FullName;
            if (!(path.EndsWith("\\") || path.EndsWith("/")))
            {
                path += "\\";
            }

            return path;
        }

        /// <summary>
        /// Ensures the folder name end with "\" for Windows platform, or "/" for Linux platform.
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static string EnsureEndWithSeperatorChar(string path, bool forWin)
        {
            DirectoryInfo di = null;
            try
            {
                di = new DirectoryInfo(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string newPath = path;

            if (forWin)
            {
                if (!newPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    newPath += Path.DirectorySeparatorChar;
                }
            }
            else
            {
                if (!newPath.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
                {
                    newPath += Path.AltDirectorySeparatorChar;
                }
            }

            return newPath;
        }
        /// <summary>
        /// Validate the file name
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Is the file name validate?</returns>
        public static bool ValidateFileName(string fileName)
        {
            bool isValid = true;
            char[] chars = Path.GetInvalidFileNameChars();
            foreach (char chr in chars)
            {
                if (fileName.Contains(chr.ToString()))
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }

        public static string ParseFileSize(int size)
        {
            if (size >= 1024 * 1024)
            {
                return Math.Round(Convert.ToDouble(size) / 1024 / 1024, 2).ToString() + "MB";
            }
            else if (size >= 1024)
            {
                return Math.Round(Convert.ToDouble(size) / 1024, 2).ToString() + "KB";
            }

            return size.ToString() + "B";
        }
    }
}
