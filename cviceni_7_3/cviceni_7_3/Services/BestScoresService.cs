using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cviceni_7_3
{
    public class BestScoresService : DbContext
    {
        string filePath;
        public DbSet<HangmanPlayerScore> BestScores { get; private set; }

        public BestScoresService(string filePath)
        {
            this.filePath = filePath;

            SQLitePCL.Batteries_V2.Init();
            Database.EnsureCreated();
            Database.Migrate();
        }

        public async Task<List<HangmanPlayerScore>> GetBestPlayersAsync()
        {
            return await BestScores.ToListAsync();
        }

        public async Task<int> SaveBestPlayerScoreAsync(HangmanPlayerScore score)
        {
            List<HangmanPlayerScore> sameOrBetterScoresForThePlayer = BestScores.Where(hp => hp.Player == score.Player && hp.Misses <= score.Misses).ToList();
            if (!sameOrBetterScoresForThePlayer.Any())
                await BestScores.AddAsync(score);
            return await SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={filePath}");
        }
    }
}
