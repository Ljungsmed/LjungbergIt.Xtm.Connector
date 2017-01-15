using LjungbergIt.Xtm.Webservice.XtmServiceReference;
using System.IO;


namespace LjungbergIt.Xtm.Webservice
{
  class XtmProjectFiles
  {
    public xtmUploadProjectFileMTOMAPI GetPreviewFile(string translationFilePath, string previewFileName)
    {
      xtmUploadProjectFileMTOMAPI previewFile = new xtmUploadProjectFileMTOMAPI();
      previewFile.fileType = xtmUPLOADPROJECTFILETYPE.PREVIEW_FILES;
      previewFile.fileTypeSpecified = true;
      previewFile.fileName = previewFileName;
      previewFile.fileMTOM = File.ReadAllBytes(translationFilePath);

      return previewFile;
    }
  }
}
