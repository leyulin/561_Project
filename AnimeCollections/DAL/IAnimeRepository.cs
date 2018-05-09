using System;
using System.Collections.Generic;
using AnimeCollections.Models;

namespace AnimeCollections.DAL
{
    public interface IAnimeRepository : IDisposable
    {
        IEnumerable<Anime> GetAnime();
        Anime GetStudentByID(int Anime);
        void InsertAnime(Anime Anime);
        void DeleteAnime(int AnimeID);
        void UpdateAnime(Anime Anime);
        void Save();
    }
}