using System;
using System.Collections.Generic;
using System.Linq;
using AnimeCollections.Models;
using AnimeCollections.Data;
using Microsoft.EntityFrameworkCore;

namespace AnimeCollections.DAL
{
    public class AnimeRepository : IAnimeRepository, IDisposable
    {
        private ApplicationDbContext context;

        public AnimeRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Anime> GetAnime()
        {
            return context.Anime.ToList();
        }

        public Anime GetStudentByID(int id)
        {
            return context.Anime.Find(id);
        }

        public void InsertAnime(Anime Anime)
        {
            context.Anime.Add(Anime);
        }

        public void DeleteAnime(int studentID)
        {
            Anime Anime = context.Anime.Find(studentID);
            context.Anime.Remove(Anime);
        }

        public void UpdateAnime(Anime Anime)
        {
            context.Entry(Anime).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}