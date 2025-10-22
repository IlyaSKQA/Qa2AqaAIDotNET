
namespace Task1.SourceCode
{
    internal class File
    {

        private readonly string _extension;
        private readonly string _fileName;
        private readonly string _content;
        private readonly double _size;

        /**
        * Construct object with passed filename and content, set extension based
        * on filename and calculate size as half content length.
        * @param filename File name (mandatory) with extension (optional), without directory tree (path separators:
        *                 https://en.wikipedia.org/wiki/Path_(computing)#Representations_of_paths_by_operating_system_and_shell)
        * @param content File content (could be empty, but must be set)
        */
        public File(string fileName, string content)
        {
            _fileName = fileName;
            _content = content;
            _size = content.Length / 2;
            _extension = fileName.Split("\\.")[fileName.Split("\\.").Length - 1];
        }

        /**
        * Get exactly file size
        * @return File size
        */
        public double GetSize()
        {
            return (int)_size;
        }

        /**
        * Get File filename
        * @return File filename
        */
        public string GetFileName()
        {
            return _fileName;
        }
    }
}
