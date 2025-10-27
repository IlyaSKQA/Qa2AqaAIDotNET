
using System.Collections.Generic;
using Task1.SourceCode.exception;

namespace Task1.SourceCode
{
    internal class FileStorage
    {
        private readonly List<File> _files = new List<File>();
        private double _availableSize = 100;
        private double _maxSize = 100;

        /**
        * Construct object and set max storage size and available size according passed values
        * @param size FileStorage size
        */
        public FileStorage(int size)
        {
            _maxSize = size;
            _availableSize += _maxSize;
        }

        /**
        * Construct object and set max storage size and available size based on default value=100
        */
        public FileStorage(){}

        /**
        * Write file in storage if filename is unique and size is not more than available size
        * @param file to save in file storage
        * @return result of file saving
        * @throws FileNameAlreadyExistsException in case of already existent filename
        */
        public bool Write(File file)
        {
            if (IsExists(file.GetFileName()))
            {
                throw new FileNameAlreadyExistsException();
            }

            if(file.GetSize() >= _availableSize)
            {
                return false;
            }

            _files.Add(file);
            _availableSize -= file.GetSize();
            return true;
        }

        /**
        * Check is file exist in storage
        * @param fileName to search
        * @return result of checking
        */
        public bool IsExists(string fileName)
        {
            foreach (var file in _files)
            {
                if (file.GetFileName().Contains(fileName))
                {
                    return true;
                }
            }
            return false;
        }

        /**
        * Delete file from storage
        * @param fileName of file to delete
        * @return result of file deleting
        */
        public bool Delete(string fileName)
        {
            return _files.Remove(GetFile(fileName));
        }

        /**
        * Get all Files saved in the storage
        * @return list of files
        */
        public List<File> GetFiles()
        {
            return _files;
        }

        /**
        * Get file by filename
        * @param fileName of file to get
        * @return file
        */
        public File GetFile(string fileName)
        {
            if (IsExists(fileName))
            {
                foreach (File file in _files) { 
                    if (file.GetFileName().Equals(fileName))
                    {
                        return file;
                    }
                }
            }
            return null;
        }
    }
}
