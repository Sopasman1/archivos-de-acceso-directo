using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archivos_de_acceso_directo
{
    internal class GameFileHandler
    {
        private string filePath;
        private int recordSize;

        public GameFileHandler(string path)
        {
            filePath = path;
            // Calculate record size:
            // int (4) + Brand (30*2) + Name (50*2) + decimal (16)
            recordSize = 4 + (Game.BrandLength * 2) + (Game.NameLength * 2) + 16;
        }

        public void Add(Game game)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                fs.Seek(0, SeekOrigin.End);

                bw.Write(game.Id);
                bw.Write(game.Brand.PadRight(Game.BrandLength).Substring(0, Game.BrandLength));
                bw.Write(game.Name.PadRight(Game.NameLength).Substring(0, Game.NameLength));
                bw.Write(game.Price);
            }
        }

        public Game Read(int recordNumber)
        {
            Game game = new Game();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader br = new BinaryReader(fs);
                fs.Seek(recordNumber * recordSize, SeekOrigin.Begin);

                game.Id = br.ReadInt32();
                game.Brand = br.ReadString().Trim();
                game.Name = br.ReadString().Trim();
                game.Price = br.ReadDecimal();
            }

            return game;
        }



        public void Update(int recordNumber, Game game)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                fs.Seek(recordNumber * recordSize, SeekOrigin.Begin);

                bw.Write(game.Id);
                bw.Write(game.Brand.PadRight(Game.BrandLength).Substring(0, Game.BrandLength));
                bw.Write(game.Name.PadRight(Game.NameLength).Substring(0, Game.NameLength));
                bw.Write(game.Price);
            }
        }

        public void Delete(int recordNumber)
        {
            Game game = Read(recordNumber);
            game.Id = -1; // Mark as deleted
            Update(recordNumber, game);
        }

        public List<Game> GetAll()
        {
            List<Game> list = new List<Game>();

            if (!File.Exists(filePath)) return list;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader br = new BinaryReader(fs);
                long recordCount = fs.Length / recordSize;

                for (int i = 0; i < recordCount; i++)
                {
                    Game game = Read(i);
                    if (game.Id > 0) // Only add if not marked as deleted
                    {
                        list.Add(game);
                    }
                }
            }

            return list;
        }

        public int RecordCount()
        {
            if (!File.Exists(filePath)) return 0;
            FileInfo fi = new FileInfo(filePath);
            return (int)(fi.Length / recordSize);
        }
    }
}
