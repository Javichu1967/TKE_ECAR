using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Web;

namespace TK_ECAR.Utils
{
    public static class FileUtilities
    {
        public static void UploadFile(HttpPostedFileBase fileToUpload, string pathToUpload )
        {
            var fileName =  GetFileName(fileToUpload);

            UploadFile(fileToUpload, pathToUpload, fileName);
        }

        public static void UploadFile(HttpPostedFileBase fileToUpload, string pathToUpload,string fileName)
        {

            CreateDirectoryIfNotExits(pathToUpload);
           
            var directorio = HttpContext.Current.Server.MapPath(pathToUpload);

            //if (!Directory.Exists(directorio))
            //{
            //    Directory.CreateDirectory(directorio);
            //}

            var path = Path.Combine(directorio, fileName);


            fileToUpload.SaveAs(path);
        }


        public static void SaveFileFromMemoryStream(MemoryStream fileToSave, string pathToUpload, string fileName)
        {

            CreateDirectoryIfNotExits(pathToUpload);

            var directorio = HttpContext.Current.Server.MapPath(pathToUpload);

            var pathAndFile = Path.Combine(directorio, fileName);

            //if (ExisteDocumentoToUploadEnDisco(Path.Combine(pathToUpload, fileName)))
            //{
            //    File.Move(pathAndFile, Path.Combine(pathToUpload, $"Copia de {fileName}"));
            //}

            using (FileStream file = new FileStream(pathAndFile, FileMode.Create, FileAccess.Write))
            {
                fileToSave.Position = 0;
                fileToSave.WriteTo(file);
            }

        }

        /// <summary>
        /// Devuelve si el documento ya existe en Disco
        /// </summary>
        /// <param name="nombreDocumento"></param>
        /// <returns></returns>
        public static bool ExisteDocumentoToUploadEnDisco(string Documento)
        {
            return (File.Exists(HttpContext.Current.Server.MapPath(Documento)));
        }


        public static string GetFileName(HttpPostedFileBase fileToUpload )
        {
            return Path.GetFileName(fileToUpload.FileName);
        }

        public static void DeleteFilesFromDirectory(string directory)
        {
            if (Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
            {
                Array.ForEach(Directory.GetFiles(HttpContext.Current.Server.MapPath(directory)), File.Delete);
            }
        }

        public static void BorraDocumentoFisico(string Archivo)
        {
            if (File.Exists(HttpContext.Current.Server.MapPath(Archivo)))
            {
                File.Delete(HttpContext.Current.Server.MapPath(Archivo));
            }
        }

        public static void RenameFile(string directory, string fileNameAnt, string fileNameNew)
        {
            string fileAnt = HttpContext.Current.Server.MapPath(directory + "/" + fileNameAnt);
            string fileNew = HttpContext.Current.Server.MapPath(directory + "/" + fileNameNew);

            if (File.Exists(fileAnt))
            {
                if (File.Exists(fileNew))
                {
                    File.Delete(fileNew);
                }

                File.Move(fileAnt, fileNew);
            }
            else
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<RenameFile> No existe el archivo origen {fileAnt}");
            }
        }

        public static void MoveFile(string DirectoryAnt, string fileNameAnt, string DirectoryNew, string fileNameNew)
        {
            string fileAnt = HttpContext.Current.Server.MapPath(DirectoryAnt + fileNameAnt);
            string fileNew = HttpContext.Current.Server.MapPath(DirectoryNew + fileNameNew);

            CreateDirectoryIfNotExits(DirectoryNew);

            if (File.Exists(fileNew))
            {
                File.Delete(fileNew);
            }

            File.Move(fileAnt, fileNew);
        }


        public static string GetVersionFile()
        {
            var valorReturn = string.Empty;

            return valorReturn;
        }
        private static void CreateDirectoryIfNotExits(string directorio)
        {
            directorio = HttpContext.Current.Server.MapPath(directorio);

            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }

        }


        /// <summary>
        /// Devuelve en un array de bytes los files comprimidos en un zip.
        /// </summary>
        /// <param name="entriesFiles">Lista de files donde contiene la ruta física y el nombre de la entrada en el zip</param>
        /// <returns></returns>
        public static byte[] CompressionFiles(List<EntryFiles> entriesFiles)
        {
            using (MemoryStream zipStream = new MemoryStream())
            {

                using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create))
                {

                    foreach (var entryFile in entriesFiles)
                    {
                        createEntryToZip(zip, entryFile.NameEntry, entryFile.Path);
                    }

                }

                return zipStream.ToArray();

            }
        }
        private static void createEntryToZip(ZipArchive zip, string nameEntry, string path)
        {
            var zipEntry = zip.CreateEntry(nameEntry);

            using (var writer = new StreamWriter(zipEntry.Open()))
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);

                var ms = new MemoryStream();

                ms.Write(bytes, 0, (int)fs.Length);

                ms.WriteTo(writer.BaseStream);

                fs.Close();
                ms.Close();
                writer.Close();
            }

        }

        public class EntryFiles
        {
            public string Path { get; set; }
            public string NameEntry { get; set; }
        }
    }
}