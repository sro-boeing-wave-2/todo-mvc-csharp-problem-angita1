using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Notes;
using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text;
using Notes;

namespace Integration.Testing
{
    public class IntegrationTest
    {
       
        private readonly HttpClient _client;
        private NotesContext _context;
        public IntegrationTest()
        {
            //Arrange 
             var  _server = new TestServer(new WebHostBuilder().UseEnvironment("Testing").UseStartup<Startup>());
            _context = _server.Host.Services.GetService(typeof(NotesContext)) as NotesContext;
            _client = _server.CreateClient();
            _context.AddRange(note1);
            _context.AddRange(note2);
            _context.AddRange(deleteNote);
            _context.SaveChanges();
        }

        Note note1 = new Note
        {

            ID = 6,
            title = "Note2",
            plainText = "This is my plaintext",
            IsPinned = true,
            label = new List<Labels>() { new Labels { value = "Label_1" }, new Labels { value = "Label_2" } },
            checklist = new List<CheckList>() { new CheckList { value = "check1", IsChecked = true } }


        };

        Note note2 = new Note
        {
            ID = 3,
            title = "Note2",
            plainText = "This is my plaintext2",
            IsPinned = true,
            label = new List<Labels>() { new Labels { value = "Label_1" }, new Labels { value = "Label_2" } },
            checklist = new List<CheckList>() { new CheckList { value = "check1", IsChecked = true } }

        };
        Note putNote = new Note
        {
            ID = 6,
            title = "Changed to NotePut",
            plainText = "This is my plaintext putttttt",
            IsPinned = true,
            label = new List<Labels>() { new Labels { value = "Label_1put" }, new Labels { value = "Label_2put" } },
            checklist = new List<CheckList>() { new CheckList { value = "check1", IsChecked = true } }

        };
        Note deleteNote = new Note
        {
            ID = 5,
            title = "Note to be deleted",
            plainText = "This is my plaintext putttttt",
            IsPinned = true,
            label = new List<Labels>() { new Labels { value = "Label_1put" }, new Labels { value = "Label_2put" } },
            checklist = new List<CheckList>() { new CheckList { value = "check1", IsChecked = true } }
        };
        Note postNote = new Note
        {
            ID = 4,
            title = "NotePost",
            plainText = "This is my plaintext postttttt",
            IsPinned = true,
            label = new List<Labels>() { new Labels { value = "Label_1put" }, new Labels { value = "Label_2put" } },
            checklist = new List<CheckList>() { new CheckList { value = "check1", IsChecked = true } }

        };

        [Fact]
        public async Task Get_AllNotes()
        {
            // Arrange
            var response = await _client.GetAsync("/api/Notes");  // type similar to whatever u type in url
                                                                  // response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Act
            var notes = JsonConvert.DeserializeObject<List<Note>>(responseString);

            //Assert
            Assert.Equal(3, notes.Count);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
        [Fact]
        public async Task Get_NoteById()
        {
            var response = await _client.GetAsync("/api/Notes/6");
            var responseString = await response.Content.ReadAsStringAsync(); //u get in json format from server as in postman
            var note = JsonConvert.DeserializeObject<Note>(responseString); //So u hv to coonvert back to type Note

            Assert.Equal("Note2",note.title);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void IntegrationTest_GetByTitle()
        {
            var response = await _client.GetAsync("/api/Notes?title=Note2");
            var responsestring = await response.Content.ReadAsStringAsync();
            var note = JsonConvert.DeserializeObject<List<Note>>(responsestring);
            //Assert
            Assert.Equal(2, note.Count);
            Assert.Equal("Note2",note[0].title);
            Assert.Equal("Note2", note[1].title);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async void IntegrationTest_GetByTitleInvalid()
        {
            var response = await _client.GetAsync("/api/Notes?title=NoteInvalid");
            var responsestring = await response.Content.ReadAsStringAsync();
            var note = JsonConvert.DeserializeObject<List<Note>>(responsestring);
            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public  async  void IntegrationTest_Post()
        {
            var postNoteContent = JsonConvert.SerializeObject(postNote); //convert to json format
            var stringContent = new StringContent(postNoteContent, Encoding.UTF8, "application/json"); //the content is is json string
            //Act
            var response = await _client.PostAsync("/api/Notes", stringContent);
            //Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var note = JsonConvert.DeserializeObject<Note>(responseString);
            Assert.Equal(4, note.ID);
           
        }
        
        [Fact]
        public async void IntegrationTest_Put()
        {
            var putNoteContent = JsonConvert.SerializeObject(putNote);
            var stringContent = new StringContent(putNoteContent, Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PutAsync("/api/Notes/6", stringContent);
            //Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var note = JsonConvert.DeserializeObject<Note>(responseString);
            
            Assert.Equal("Changed to NotePut", note.title);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task IntegrationTest_PutNoteInvalid()
        {
            var json = JsonConvert.SerializeObject(putNote);
            var stringcontent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Notes/16", stringcontent);
            var responsecontent = await response.Content.ReadAsStringAsync();
            var responsenote = JsonConvert.DeserializeObject<Note>(responsecontent);
            var responsedata = response.StatusCode;
            Assert.Equal(HttpStatusCode.NotFound, responsedata);
        }
        [Fact]
        public async void IntegrationTest_Delete()
        {
            var responseD = await _client.DeleteAsync("/api/Notes/5");
            responseD.EnsureSuccessStatusCode();
            var responseStringD = await responseD.Content.ReadAsStringAsync();
            //Assert.Null(responseString);

            //Get all notes
            var response = await _client.GetAsync("/api/Notes");  // type similar to whatever u type in url
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<List<Note>>(responseString);

            Assert.Equal(2, notes.Count);  //Check the count of notes after one note got deleted
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }





    }
}
