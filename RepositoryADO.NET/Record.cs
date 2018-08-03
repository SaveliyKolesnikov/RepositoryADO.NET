using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryADO.NET
{
    public class Record
    {
        public Record(int id, string text, string author, DateTime recoredDate)
        {
            Id = id;
            Text = text;
            Author = author;
            RecordDate = recoredDate;
        }

        public int Id { get; }
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime RecordDate { get; set; }

        public override string ToString()
        {
            return $"{Id}\t{Text}\t{Author}\t{RecordDate}";
        }
    }
}
