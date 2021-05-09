using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using YFM.EntityModels;
using YFM.Models;

namespace YFM.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper instance
        public IMapper mapper;

        // Request user property...

        // Backing field for the property
        private RequestUser _user;

        // Getter only, no setter
        public RequestUser User
        {
            get
            {
                // On first use, it will be null, so set its value
                if (_user == null)
                {
                    _user = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);
                }
                return _user;
            }
        }

        // Default constructor...
        public Manager()
        {
            // If necessary, add constructor code here

            // Configure the AutoMapper components
            var config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                // cfg.CreateMap<Employee, EmployeeBase>();

                // Object mapper definitions

                cfg.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();

                cfg.CreateMap<Genre, GenreBaseViewModel>();

                cfg.CreateMap<Artist, ArtistAddViewModel>();
                cfg.CreateMap<Artist, ArtistBaseViewModel>();
                cfg.CreateMap<ArtistAddViewModel, Artist>();

                cfg.CreateMap<Artist, ArtistWithMediaItemViewModel>();

                cfg.CreateMap<Album, AlbumAddViewModel>();
                cfg.CreateMap<AlbumAddViewModel, Album>();
                cfg.CreateMap<Album, AlbumBaseViewModel>();

                cfg.CreateMap<Track, TrackAddViewModel>();
                cfg.CreateMap<TrackAddViewModel, Track>();
                cfg.CreateMap<Track, TrackBaseViewModel>();
                cfg.CreateMap<Track, TrackWithAudioDetailsViewModel>();
                cfg.CreateMap<TrackWithAudioDetailsViewModel, Track>();
                cfg.CreateMap<Track, TrackClipViewModel>();
                cfg.CreateMap<Track, TrackEditClipViewModel>();
                cfg.CreateMap<TrackBaseViewModel, TrackEditClipFormViewModel>();
                cfg.CreateMap<TrackWithAudioDetailsViewModel, TrackEditClipFormViewModel>();

                cfg.CreateMap<MediaItem, MediaItemBaseViewModel>();
                cfg.CreateMap<MediaItem, MediaItemAddViewModel>();
                cfg.CreateMap<MediaItemAddViewModel, MediaItem>();
                cfg.CreateMap<MediaItem, MediaItemContentViewModel>();
                
            });

            mapper = config.CreateMapper();

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }

        // ############################################################
        // RoleClaim

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        // Add methods below
        // Controllers will call these methods
        // Ensure that the methods accept and deliver ONLY view model objects and collections
        // The collection return type is almost always IEnumerable<T>

        // Suggested naming convention: Entity + task/action
        // For example:
        // ProductGetAll()
        // ProductGetById()
        // ProductAdd()
        // ProductEdit()
        // ProductDelete()

        public IEnumerable<GenreBaseViewModel> GetAllGenres()
        {
            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBaseViewModel>>(ds.Genres.OrderBy(g => g.Name));
        }
        public IEnumerable<ArtistBaseViewModel> GetAllArtists()
        {
            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(ds.Artists.OrderBy(a => a.Name));
        }

        public IEnumerable<AlbumBaseViewModel> GetAllAlbums()
        {
            return mapper.Map<IEnumerable<Album>, IEnumerable<AlbumBaseViewModel>>(ds.Albums.OrderBy(a => a.Name));
        }

        public IEnumerable<TrackBaseViewModel> GetAllTracks()
        {
            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackBaseViewModel>>(ds.Tracks.OrderBy(t => t.Name));
        }

        public ArtistBaseViewModel GetArtistById(int id)
        {
            var a = ds.Artists.Find(id);
            return (a == null) ? null : mapper.Map<Artist, ArtistBaseViewModel>(a);
        }

        public AlbumBaseViewModel GetAlbumById(int id)
        {
            var a = ds.Albums.Find(id);
            return (a == null) ? null : mapper.Map<Album, AlbumBaseViewModel>(a);
        }

        public TrackWithAudioDetailsViewModel GetTrackById(int id)
        {
            var t = ds.Tracks.Find(id);
            return (t == null) ? null : mapper.Map<Track, TrackWithAudioDetailsViewModel>(t);
        }

        public TrackClipViewModel GetTrackClipById(int id)
        {
            var o = ds.Tracks.Find(id);
            return (o == null) ? null : mapper.Map<Track, TrackClipViewModel>(o);
        }

        public ArtistBaseViewModel AddNewArtist(ArtistAddViewModel newItem)
        {
            var u = HttpContext.Current.User.Identity.Name;
            var g = ds.Genres.Find(newItem.GenreId);
            if (g == null)
            {
                return null;
            }
            else
            {
                var addedItem = ds.Artists.Add(mapper.Map<ArtistAddViewModel, Artist>(newItem));
                addedItem.Genre = g.Name;
                addedItem.Executive = u;
                ds.SaveChanges();
                return addedItem == null ? null : mapper.Map<Artist, ArtistBaseViewModel>(addedItem);
            }
        }
        public AlbumBaseViewModel AddNewAlbum(AlbumAddViewModel newItem)
        {
            var u = HttpContext.Current.User.Identity.Name;
            var a = ds.Artists.Find(newItem.ArtistId);
            var g = ds.Genres.Find(newItem.GenreId);

            if (a == null || g == null)
            {
                return null;
            }
            else
            {

                var addedItem = ds.Albums.Add(mapper.Map<AlbumAddViewModel, Album>(newItem));
                addedItem.Artists.Add(a);
                addedItem.Genre = g.Name;
                addedItem.Coordinator = u;
                ds.SaveChanges();

                return addedItem == null ? null : mapper.Map<Album, AlbumBaseViewModel>(addedItem);
            }
        }

        public TrackWithAudioDetailsViewModel AddNewTrack(TrackAddViewModel newItem)
        {
            var u = HttpContext.Current.User.Identity.Name;
            var a = ds.Albums.Find(newItem.AlbumId);
            var g = ds.Genres.Find(newItem.GenreId);

            if (a == null || g == null || u == null)
            {
                return null;
            }
            else
            {
                var addedItem = ds.Tracks.Add(mapper.Map<TrackAddViewModel, Track>(newItem));
                addedItem.Albums.Add(a);
                addedItem.Genre = g.Name;
                addedItem.Clerk = u;
                byte[] clipBytes = new byte[newItem.ClipUpload.ContentLength];
                newItem.ClipUpload.InputStream.Read(clipBytes, 0, newItem.ClipUpload.ContentLength);

                addedItem.SampleClip = clipBytes;
                addedItem.ClipContentType = newItem.ClipUpload.ContentType;

                ds.SaveChanges();

                return (addedItem == null) ? null : mapper.Map<Track, TrackWithAudioDetailsViewModel>(addedItem);
            }
        }

        public TrackWithAudioDetailsViewModel EditTrack(TrackEditClipViewModel newItem)
        {
            var track = ds.Tracks.SingleOrDefault(t => t.Id == newItem.Id);
            if (track == null)
            {
                return null;
            }
            else
            {
                byte[] clipBytes = new byte[newItem.ClipUpload.ContentLength];
                newItem.ClipUpload.InputStream.Read(clipBytes, 0, newItem.ClipUpload.ContentLength);
                track.SampleClip = clipBytes;
                track.ClipContentType = newItem.ClipUpload.ContentType;
                ds.Entry(track).CurrentValues.SetValues(newItem);
                ds.SaveChanges();

                return mapper.Map<Track, TrackWithAudioDetailsViewModel>(track);
            }
        }

        public bool TrackDelete(int id)
        {
            var itemToDelete = ds.Tracks.Find(id);
            if (itemToDelete == null)
            {
                return false;
            }
            else
            {
                try
                {
                    ds.Tracks.Remove(itemToDelete);
                    ds.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        
        public ArtistBaseViewModel AddMediaItemToArtist(MediaItemAddViewModel newItem)
        {
            var a = ds.Artists.Find(newItem.ArtistId);
            if (a == null)
            {
                return null;
            }
            else
            {
                var addedItem = new MediaItem();
                ds.MediaItems.Add(addedItem);
                addedItem.Caption = newItem.Caption;
                addedItem.Artist = a;
                byte[] mediaByte = new byte[newItem.MediaUpload.ContentLength];
                newItem.MediaUpload.InputStream.Read(mediaByte, 0, newItem.MediaUpload.ContentLength);

                addedItem.Content = mediaByte;
                addedItem.ContentType = newItem.MediaUpload.ContentType;

                ds.SaveChanges();

                return (addedItem == null) ? null : mapper.Map<Artist, ArtistBaseViewModel>(a);

            }
        }

        public ArtistWithMediaItemViewModel GetArtistWithMediaItemById(int id)
        {
            var o = ds.Artists.Include("MediaItems").SingleOrDefault(m => m.Id == id);
            return (o == null) ? null : mapper.Map<Artist, ArtistWithMediaItemViewModel>(o);
        }

        public MediaItemContentViewModel GetArtistMediaItemById(string stringId)
        {
            var o = ds.MediaItems.SingleOrDefault(p => p.StringId == stringId);
            return (o == null) ? null : mapper.Map<MediaItem, MediaItemContentViewModel>(o);
        }



        // Add some programmatically-generated objects to the data store
        // Can write one method, or many methods - your decision
        // The important idea is that you check for existing data first
        // Call this method from a controller action/method

        public bool LoadData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // ############################################################
            // Role claims

            if (ds.RoleClaims.Count() == 0)
            {
                // Add role claims here
                ds.RoleClaims.Add(new RoleClaim { Name = "Clerk" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Coordinator" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Executive" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Staff" });

                ds.SaveChanges();
                done = true;
            }

            // ############################################################
            // Genre

            if (ds.Genres.Count() == 0)
            {
                // Add genres

                ds.Genres.Add(new Genre { Name = "Folk" });
                ds.Genres.Add(new Genre { Name = "Classical" });
                ds.Genres.Add(new Genre { Name = "Country" });
                ds.Genres.Add(new Genre { Name = "Easy Listening" });
                ds.Genres.Add(new Genre { Name = "Hip-Hop/Rap" });
                ds.Genres.Add(new Genre { Name = "Jazz" });
                ds.Genres.Add(new Genre { Name = "Pop" });
                ds.Genres.Add(new Genre { Name = "R&B" });
                ds.Genres.Add(new Genre { Name = "Rock" });
                ds.Genres.Add(new Genre { Name = "Soundtrack" });

                ds.SaveChanges();
                done = true;
            }

            // ############################################################
            // Artist

            if (ds.Artists.Count() == 0)
            {
                // Add artists

                ds.Artists.Add(new Artist
                {
                    Name = "Joan Baez",
                    BirthOrStartDate = new DateTime(1941, 1, 9),
                    Executive = user,
                    Genre = "Folk",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/Joan_Baez_at_the_The_Egg_%28Albany%2C_NY%29%2C_March_2016.jpg/260px-Joan_Baez_at_the_The_Egg_%28Albany%2C_NY%29%2C_March_2016.jpg"
                });

                ds.Artists.Add(new Artist
                {
                    Name = "The Bangles",
                    BirthOrStartDate = new DateTime(1981, 12, 5),
                    Executive = user,
                    Genre = "Pop",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f0/Bangles_at_Festival_of_Friends_2012.jpg/300px-Bangles_at_Festival_of_Friends_2012.jpg"
                });

                ds.SaveChanges();
                done = true;
            }

            // ############################################################
            // Album

            if (ds.Albums.Count() == 0)
            {
                // Add albums

                // For Joan Bauz
                var jb = ds.Artists.SingleOrDefault(a => a.Name == "Joan Baez");

                ds.Albums.Add(new Album
                {
                    Artists = new List<Artist> { jb },
                    Name = "Diamonds and Rust in the Bullring",
                    ReleaseDate = new DateTime(1988, 12, 4),
                    Coordinator = user,
                    Genre = "Folk",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/thumb/3/34/Baezbullring.jpg/220px-Baezbullring.jpg"
                });

                // For The Bangles
                var tb = ds.Artists.SingleOrDefault(a => a.Name == "The Bangles");
                ds.Albums.Add(new Album
                {
                    Artists = new List<Artist> { tb },
                    Name = "Everything",
                    ReleaseDate = new DateTime(1988, 10, 18),
                    Coordinator = user,
                    Genre = "Rock",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/thumb/f/f8/The_Bangles_-_Everything.jpg/220px-The_Bangles_-_Everything.jpg"
                });

                ds.SaveChanges();
                done = true;
            }

            // ############################################################
            // Track

            if (ds.Tracks.Count() == 0)
            {
                // Add tracks

                // For Diamonds & Rust in the Bullring （+2）
                var drb = ds.Albums.SingleOrDefault(a => a.Name == "Diamonds and Rust in the Bullring");

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { drb },
                    Name = "Diamond and Rust",
                    Composers = "Joan Baez",
                    Clerk = user,
                    Genre = "Folk"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { drb },
                    Name = "No Woman, No Cry",
                    Composers = "Joan Baez, Bob Marley",
                    Clerk = user,
                    Genre = "Folk"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { drb },
                    Name = "Famous Blue Raincoat",
                    Composers = "Joan Baez, Leonard Cohen",
                    Clerk = user,
                    Genre = "Folk"
                });

                // For Everything （+2）
                var e = ds.Albums.SingleOrDefault(a => a.Name == "Everything");

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { e },
                    Name = "Glitter Years",
                    Composers = "The Bangles",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { e },
                    Name = "Eternal Flame",
                    Composers = "The Bangles",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.SaveChanges();
                done = true;
            }

            return done;
        }

        public bool RemoveData()
        {
            try
            {
                foreach (var e in ds.RoleClaims)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Tracks)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Albums)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Artists)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Genres)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    // New "RequestUser" class for the authenticated user
    // Includes many convenient members to make it easier to render user account info
    // Study the properties and methods, and think about how you could use it

    // How to use...

    // In the Manager class, declare a new property named User
    //public RequestUser User { get; private set; }

    // Then in the constructor of the Manager class, initialize its value
    //User = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);

    public class RequestUser
    {
        // Constructor, pass in the security principal
        public RequestUser(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                // You can change the string value in your app to match your app domain logic
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            // Compose the nicely-formatted full names
            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }
        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }
        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }
        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }
}