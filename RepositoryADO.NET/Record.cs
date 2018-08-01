using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryADO.NET
{
    public class Record
    {
        public int Id { set; get; }
        public string Text { set; get; }
        public string Author { set; get; }
        public DateTime RecordDate { set; get; }

        public override string ToString()
        {
            return $"{Id}\t{Text}\t{Author}\t{RecordDate}";
        }
    }
}
