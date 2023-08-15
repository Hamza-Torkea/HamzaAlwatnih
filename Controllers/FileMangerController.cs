using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using alwatnia.Helper;

namespace alwatnia.Controllers
{
    public class FilemanagerController : Controller
    {
        private readonly FilemanagerConfig config = new FilemanagerConfig();
        private readonly JavaScriptSerializer json = new JavaScriptSerializer();

        public ActionResult Index(string mode, string path = null)
        {
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Clear();
            try
            {
                switch (mode)
                {
                    case "getinfo":
                        return Content(GetInfo(path), "application/json", Encoding.UTF8);
                    case "getfolder":
                        return Content(GetFolderInfo(path), "application/json", Encoding.UTF8);
                    case "move":
                        var str = Request.QueryString["old"];
                        var newPath = string.Format("{0}{1}/{2}", Request.QueryString["root"],
                            Request.QueryString["new"], Path.GetFileName(str));
                        return Content(Move(str, newPath), "application/json", Encoding.UTF8);
                    case "rename":
                        return Content(Rename(Request.QueryString["old"], Request.QueryString["new"]),
                            "application/json", Encoding.UTF8);
                    case "replace":
                        return Content(Replace(Request.Form["newfilepath"]), "text/html", Encoding.UTF8);
                    case "delete":
                        return Content(Delete(path), "application/json", Encoding.UTF8);
                    case "addfolder":
                        return Content(AddFolder(path, Request.QueryString["name"]), "application/json", Encoding.UTF8);
                    case "download":
                        if (!System.IO.File.Exists(Server.MapPath(path)) || !IsInRootPath(path))
                            return new HttpNotFoundResult("File not found");
                        var fileInfo1 = new FileInfo(Server.MapPath(path));
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlPathEncode(path));
                        Response.AddHeader("Content-Length", fileInfo1.Length.ToString());
                        return File(fileInfo1.FullName, "application/octet-stream");
                    case "add":
                        return Content(AddFile(Request.Form["currentpath"]), "text/html", Encoding.UTF8);
                    case "preview":
                        var fileInfo2 = new FileInfo(Server.MapPath(Request.QueryString[nameof(path)]));
                        return new FilePathResult(fileInfo2.FullName, "image/" + fileInfo2.Extension.TrimStart('.'));
                    default:
                        return Content(Error(string.Format("{0} not implemented", mode)));
                }
            }
            catch (HttpException ex)
            {
                return Content(Error(ex.Message), "application/json", Encoding.UTF8);
            }
        }

        private bool IsImage(FileInfo fileInfo)
        {
            return config.ImgExtensions.Contains(Path.GetExtension(fileInfo.FullName).ToLower());
        }

        private bool IsInRootPath(string path)
        {
            return path != null && Path.GetFullPath(path).StartsWith(Path.GetFullPath(config.RootPath));
        }

        private string AddFile(string path)
        {
            string str;
            if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
            {
                str = Error("No file provided.");
            }
            else if (!IsInRootPath(path))
            {
                str = Error("Attempt to add file outside root path");
            }
            else
            {
                var file = Request.Files[0];
                if (!config.AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                {
                    str = Error("Uploaded file type is not allowed.");
                }
                else
                {
                    var input = Regex.Replace(Path.GetFileNameWithoutExtension(file.FileName), "[^\\w_-]", "");
                    var path1 = Path.Combine(path, input + Path.GetExtension(file.FileName));
                    var num = 0;
                    string path1_1;
                    string path2;
                    for (; System.IO.File.Exists(Server.MapPath(path1)); path1 = Path.Combine(path1_1, path2))
                    {
                        ++num;
                        input = Regex.Replace(input, "_[\\d]+$", "");
                        path1_1 = path;
                        path2 = input + "_" + num + Path.GetExtension(file.FileName);
                    }
                    file.SaveAs(Server.MapPath(path1));
                    str = json.Serialize(new
                    {
                        Path = path,
                        Name = Path.GetFileName(file.FileName),
                        Error = "No error",
                        Code = 0
                    });
                }
            }
            return "<textarea>" + str + "</textarea>";
        }

        private string AddFolder(string path, string newFolder)
        {
            if (!IsInRootPath(path))
                return Error("Attempt to add folder outside root path");
            Directory.CreateDirectory(Path.Combine(Server.MapPath(path), newFolder));
            return json.Serialize(new
            {
                Parent = path,
                Name = newFolder,
                Error = "",
                Code = 0
            });
        }

        private string Delete(string path)
        {
            if (!IsInRootPath(path))
                return Error("Attempt to delete file outside root path");
            if (!System.IO.File.Exists(Server.MapPath(path)) && !Directory.Exists(Server.MapPath(path)))
                return Error("File not found");
            if ((System.IO.File.GetAttributes(Server.MapPath(path)) & FileAttributes.Directory) ==
                FileAttributes.Directory)
                Directory.Delete(Server.MapPath(path), true);
            else
                System.IO.File.Delete(Server.MapPath(path));
            return json.Serialize(new
            {
                Path = path,
                Error = "",
                Code = 0
            });
        }

        private string Error(string msg)
        {
            return json.Serialize(new
            {
                Error = msg,
                Code = -1
            });
        }

        private Dictionary<string, object> GetDirectoryInfo(DirectoryInfo dirInfo, string fullPath)
        {
            return new Dictionary<string, object>
            {
                {
                    "Path",
                    fullPath
                },
                {
                    "Filename",
                    dirInfo.Name
                },
                {
                    "File Type",
                    "dir"
                },
                {
                    "Protected",
                    dirInfo.Attributes.HasFlag(FileAttributes.ReadOnly) ? 1 : 0
                },
                {
                    "Preview",
                    config.IconDirectory + "_Open.png"
                },
                {
                    "Properties",
                    new Dictionary<string, object>
                    {
                        {
                            "Date Created",
                            dirInfo.CreationTime.ToString()
                        },
                        {
                            "Date Modified",
                            dirInfo.LastWriteTime.ToString()
                        },
                        {
                            "Height",
                            0
                        },
                        {
                            "Width",
                            0
                        },
                        {
                            "Size",
                            0
                        }
                    }
                },
                {
                    "Error",
                    ""
                },
                {
                    "Code",
                    0
                }
            };
        }

        private Dictionary<string, object> GetFileInfo(FileInfo fileInfo, string fullPath)
        {
            var num1 = 0;
            var num2 = 0;
            string path;
            if (IsImage(fileInfo))
            {
                path = fullPath + "?" + fileInfo.LastWriteTime.Ticks;
            }
            else
            {
                path = string.Format("{0}{1}.png", config.IconDirectory, fileInfo.Extension.Replace(".", ""));
                if (!System.IO.File.Exists(Server.MapPath(path)))
                    path = string.Format("{0}default.png", config.IconDirectory);
            }
            if (IsImage(fileInfo))
                try
                {
                    using (var image = Image.FromFile(fileInfo.FullName))
                    {
                        num1 = image.Height;
                        num2 = image.Width;
                    }
                }
                catch
                {
                }
            return new Dictionary<string, object>
            {
                {
                    "Path",
                    fullPath
                },
                {
                    "Filename",
                    fileInfo.Name
                },
                {
                    "File Type",
                    fileInfo.Extension.Replace(".", "")
                },
                {
                    "Protected",
                    fileInfo.IsReadOnly ? 1 : 0
                },
                {
                    "Preview",
                    path
                },
                {
                    "Properties",
                    new Dictionary<string, object>
                    {
                        {
                            "Date Created",
                            fileInfo.CreationTime.ToString()
                        },
                        {
                            "Date Modified",
                            fileInfo.LastWriteTime.ToString()
                        },
                        {
                            "Height",
                            num1
                        },
                        {
                            "Width",
                            num2
                        },
                        {
                            "Size",
                            fileInfo.Length
                        }
                    }
                },
                {
                    "Error",
                    ""
                },
                {
                    "Code",
                    0
                }
            };
        }

        private string GetFolderInfo(string path)
        {
            if (!IsInRootPath(path))
                return Error("Attempt to view files outside root path");
            if (!Directory.Exists(Server.MapPath(path)))
                return Error("Directory not found");
            var directoryInfo = new DirectoryInfo(Server.MapPath(path));
            var dictionary = new Dictionary<string, object>();
            foreach (var directory in directoryInfo.GetDirectories())
            {
                var key = Path.Combine(path, directory.Name);
                dictionary.Add(key, GetDirectoryInfo(directory, key + "/"));
            }
            foreach (var file in directoryInfo.GetFiles())
            {
                var str = Path.Combine(path, file.Name);
                dictionary.Add(str, GetFileInfo(file, str));
            }
            return json.Serialize(dictionary);
        }

        private string GetInfo(string path)
        {
            if (!IsInRootPath(path))
                return Error("Attempt to view file outside root path");
            if (!System.IO.File.Exists(Server.MapPath(path)) && !Directory.Exists(Server.MapPath(path)))
                return Error("File not found");
            if ((System.IO.File.GetAttributes(Server.MapPath(path)) & FileAttributes.Directory) ==
                FileAttributes.Directory)
                return json.Serialize(GetDirectoryInfo(new DirectoryInfo(Server.MapPath(path)), path));
            return json.Serialize(GetFileInfo(new FileInfo(Server.MapPath(path)), path));
        }

        private string Move(string oldPath, string newPath)
        {
            if (!IsInRootPath(oldPath))
                return Error("Attempt to modify file outside root path");
            if (!IsInRootPath(newPath))
                return Error("Attempt to move a file outside root path");
            if (!System.IO.File.Exists(Server.MapPath(oldPath)) && !Directory.Exists(Server.MapPath(oldPath)))
                return Error("File not found");
            if ((System.IO.File.GetAttributes(Server.MapPath(oldPath)) & FileAttributes.Directory) ==
                FileAttributes.Directory)
            {
                var directoryInfo1 = new DirectoryInfo(Server.MapPath(oldPath));
                newPath = Path.Combine(newPath, directoryInfo1.Name);
                Directory.Move(Server.MapPath(oldPath), Server.MapPath(newPath));
                var directoryInfo2 = new DirectoryInfo(Server.MapPath(newPath));
                return json.Serialize(new Dictionary<string, object>
                {
                    {
                        "Old Path",
                        oldPath
                    },
                    {
                        "Old Name",
                        directoryInfo1.Name
                    },
                    {
                        "New Path",
                        directoryInfo2.FullName.Replace(HttpRuntime.AppDomainAppPath, "/")
                            .Replace(Path.DirectorySeparatorChar, '/')
                    },
                    {
                        "New Name",
                        directoryInfo2.Name
                    },
                    {
                        "Error",
                        ""
                    },
                    {
                        "Code",
                        0
                    }
                });
            }
            var fileInfo1 = new FileInfo(Server.MapPath(oldPath));
            var fileInfo2 = new FileInfo(Server.MapPath(newPath));
            if (fileInfo2.Extension != fileInfo1.Extension)
                fileInfo2 = new FileInfo(Path.ChangeExtension(fileInfo2.FullName, fileInfo1.Extension));
            System.IO.File.Move(fileInfo1.FullName, fileInfo2.FullName);
            return json.Serialize(new Dictionary<string, object>
            {
                {
                    "Old Path",
                    oldPath.Replace(fileInfo1.Name, "")
                },
                {
                    "Old Name",
                    fileInfo1.Name
                },
                {
                    "New Path",
                    fileInfo2.FullName.Replace(HttpRuntime.AppDomainAppPath, "/")
                        .Replace(Path.DirectorySeparatorChar, '/').Replace(fileInfo2.Name, "")
                },
                {
                    "New Name",
                    fileInfo2.Name
                },
                {
                    "Error",
                    ""
                },
                {
                    "Code",
                    0
                }
            });
        }

        private string Rename(string path, string newName)
        {
            if (!IsInRootPath(path))
                return Error("Attempt to modify file outside root path");
            if (!System.IO.File.Exists(Server.MapPath(path)) && !Directory.Exists(Server.MapPath(path)))
                return Error("File not found");
            if ((System.IO.File.GetAttributes(Server.MapPath(path)) & FileAttributes.Directory) ==
                FileAttributes.Directory)
            {
                var directoryInfo1 = new DirectoryInfo(Server.MapPath(path));
                Directory.Move(Server.MapPath(path), Path.Combine(directoryInfo1.Parent.FullName, newName));
                var directoryInfo2 = new DirectoryInfo(Path.Combine(directoryInfo1.Parent.FullName, newName));
                return json.Serialize(new Dictionary<string, object>
                {
                    {
                        "Old Path",
                        path
                    },
                    {
                        "Old Name",
                        directoryInfo1.Name
                    },
                    {
                        "New Path",
                        directoryInfo2.FullName.Replace(HttpRuntime.AppDomainAppPath, "/")
                            .Replace(Path.DirectorySeparatorChar, '/')
                    },
                    {
                        "New Name",
                        directoryInfo2.Name
                    },
                    {
                        "Error",
                        ""
                    },
                    {
                        "Code",
                        0
                    }
                });
            }
            var fileInfo1 = new FileInfo(Server.MapPath(path));
            newName = Path.GetFileNameWithoutExtension(newName) + fileInfo1.Extension;
            var fileInfo2 = new FileInfo(Path.Combine(fileInfo1.Directory.FullName, newName));
            System.IO.File.Move(fileInfo1.FullName, fileInfo2.FullName);
            return json.Serialize(new Dictionary<string, object>
            {
                {
                    "Old Path",
                    path
                },
                {
                    "Old Name",
                    fileInfo1.Name
                },
                {
                    "New Path",
                    fileInfo2.FullName.Replace(HttpRuntime.AppDomainAppPath, "/")
                        .Replace(Path.DirectorySeparatorChar, '/')
                },
                {
                    "New Name",
                    fileInfo2.Name
                },
                {
                    "Error",
                    ""
                },
                {
                    "Code",
                    0
                }
            });
        }

        private string Replace(string path)
        {
            if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
                return Error("No file provided.");
            if (!IsInRootPath(path))
                return Error("Attempt to replace file outside root path");
            var fileInfo = new FileInfo(Server.MapPath(path));
            var file = Request.Files[0];
            if (!config.AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                return Error("Uploaded file type is not allowed.");
            if (!Path.GetExtension(file.FileName).Equals(fileInfo.Extension))
                return Error("Replacement file must have the same extension as the file being replaced.");
            if (!fileInfo.Exists)
                return Error("File to replace not found.");
            file.SaveAs(fileInfo.FullName);
            return "<textarea>" + json.Serialize(new
            {
                Path = path.Replace("/" + fileInfo.Name, ""),
                fileInfo.Name,
                Error = "No error",
                Code = 0
            }) + "</textarea>";
        }
    }
}