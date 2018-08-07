using Notes.Controllers;
using System;
using Xunit;
using NSuperTest;
using System.Linq;
using Notes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace NoteController.Testing
{
    public class NoteControllerTest
    {
       
        public NotesController GetNewController()
        {
            var optionBuilder = new DbContextOptionsBuilder<NotesContext>();
            optionBuilder.UseInMemoryDatabase<NotesContext>(Guid.NewGuid().ToString());
            NotesContext notesContext = new NotesContext(optionBuilder.Options);
            CreateNotes(optionBuilder.Options);
            return new NotesController(notesContext);
            
        }

        public void CreateNotes(DbContextOptions<NotesContext> notes_Context)
        {
            //Arrange
            using (var notesContext = new NotesContext(notes_Context))
            {
                var notes = new List<Note>()
               {
                // 1st note
                new Note()
                {
                   ID = 6,
                   title="Note1",
                   plainText="This is my plaintext",
                   IsPinned=true,
                   label = new List<Labels>() { new Labels { value ="Label_1" },new Labels { value = "Label_2" } },
                   checklist=new List<CheckList>() { new CheckList { value ="check1",IsChecked = true} }

                },
                new Note()
                {
                   ID = 7,
                   title="Note12",
                   plainText="This is my plaintext 2",
                   IsPinned=true,
                   label = new List<Labels>() { new Labels { value ="Label_1" },new Labels { value = "Label_2" } },
                   checklist=new List<CheckList>() { new CheckList { value ="check1",IsChecked = true} }

                },
                 new Note()
                {
                   ID = 9,
                   title="Note3",
                   plainText="This is my plaintext 3",
                   IsPinned=true,
                   label = new List<Labels>() { new Labels { value ="Label_1" },new Labels { value = "Label_2" } },
                   checklist=new List<CheckList>() { new CheckList { value ="check1",IsChecked = true} }

                }

            };

               notesContext.Note.AddRange(notes);
                var CountNotes = notesContext.ChangeTracker.Entries().Count();
                notesContext.SaveChanges();
            }
        }
        /// <summary>
        /// Get Tests
        /// </summary>
        [Fact]
        public async void TestGetAllNotes()
        {
            var notesController = GetNewController(); //create a new controller
            var okResult = await notesController.GetAllNotes() as OkObjectResult;
            var result = okResult.Value as List<Note>;
           // Console.WriteLine(result);
            Assert.Equal(3,result.Count);
        }


        /// <summary>
        /// Get ById Test
        /// </summary>
        [Fact]
        public async void Test_GetById()
        {
            var notesController = GetNewController();
            var okResult = await notesController.GetNoteById(9) as OkObjectResult;
            var result = okResult.Value as Note;
            Console.WriteLine(result);
            Assert.Equal(9, result.ID);
           
        }


        /// <summary>
        /// Post Test
        /// </summary>
        [Fact]
        public void TestPost()
        { 
            //Arrange 
            var noteCreated = new Note
            {
                ID = 5,
                title = "NoteCreation",
                plainText = "This is my plaintext1",
                IsPinned = true,
                label = new List<Labels>() { new Labels { value = "Label_1" }, new Labels { value = "Label_2" } },
                checklist = new List<CheckList>() { new CheckList { value = "check1", IsChecked = true } }

            };
            //Act
            var notesController = GetNewController();
            var Response = notesController.PostNote(noteCreated).Result as CreatedAtActionResult;          
            var item = Response.Value as Note;

            // Assert
            Assert.IsType<Note>(item);
            Assert.Equal("NoteCreation", item.title);
        }


        /// <summary>
        /// Delete ById Test
        /// </summary>
        [Fact]
        public async void Test_Delete_NoteById()
        {

            // Arrange
            var notesController = GetNewController();
            var okResponse = await notesController.DeleteNote(7) as OkObjectResult;
            //Act
            var result = okResponse.Value as Note;
            
            //Assert
            // Assert.IsType<OkResult>(okResponse);
            Assert.Equal(7, result.ID);


        }


        /// <summary>
        /// Put Test
        /// </summary>
        [Fact]
        public async void Test_Put()
        {
            var notePut = new Note
            {   
                ID = 6,
                title = "Note Changed",
                plainText = "This is my plaintextttt",
                IsPinned = true,
                label = new List<Labels>() { new Labels { value = "Label_1" }, new Labels { value = "Label_2" } },
                checklist = new List<CheckList>() { new CheckList { value = "check1", IsChecked = true } }
      
            };
            var notesController = GetNewController();
            var okResponse = await notesController.PutNote(6, notePut);
            var okRespopnseResult = okResponse as OkObjectResult;
            var result = okRespopnseResult.Value as Note;
            Assert.Equal("Note Changed", result.title);

        }

    }
}
