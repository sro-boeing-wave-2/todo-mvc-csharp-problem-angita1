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
        private  NotesController notesController;
        public NoteControllerTest()
        {
            var optionBuilder = new DbContextOptionsBuilder<NotesContext>();
            optionBuilder.UseInMemoryDatabase("Some database");
            NotesContext notesContext = new NotesContext(optionBuilder.Options);
            notesController = new NotesController(notesContext);
            CreateNotes(notesContext);
        }
        
        public void CreateNotes(NotesContext notes_Context)
        {
            //Arrange
            var notes = new List<Note>()
            {
                // 1st note
                new Note
                {
                 
                   title="Note1",
                   plainText="This is my plaintext",
                   IsPinned=true,
                   label = new List<Labels>() { new Labels { value ="Label_1" },new Labels { value = "Label_2" } },
                   checklist=new List<CheckList>() { new CheckList { value ="check1",IsChecked = true} }

                },
                new Note
                {

                   title="Note12",
                   plainText="This is my plaintext 2",
                   IsPinned=true,
                   label = new List<Labels>() { new Labels { value ="Label_1" },new Labels { value = "Label_2" } },
                   checklist=new List<CheckList>() { new CheckList { value ="check1",IsChecked = true} }

                },
                 new Note
                {

                   title="Note3",
                   plainText="This is my plaintext 3",
                   IsPinned=true,
                   label = new List<Labels>() { new Labels { value ="Label_1" },new Labels { value = "Label_2" } },
                   checklist=new List<CheckList>() { new CheckList { value ="check1",IsChecked = true} }

                }

            };

            notes_Context.AddRange(notes);
            notes_Context.SaveChanges();           
        }
        /// <summary>
        /// Get Tests
        /// </summary>
        [Fact]
        public async void TestGetAllNotes()
        {
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
            var okResult = await notesController.GetNoteById(2) as OkObjectResult;
            var result = okResult.Value as Note;
            Console.WriteLine(result);
            Assert.Equal(2, result.ID);
            Assert.Equal("Note12",result.title);
        }


        /// <summary>
        /// Post Test
        /// </summary>
        [Fact]
        public void TestPost()
        { 
            //Arrange 
            var noteCreated = new Note()
            {
                
                title = "NoteCreation",
                plainText = "This is my plaintext1",
                IsPinned = true,
                label = new List<Labels>() { new Labels { value = "Label_1" }, new Labels { value = "Label_2" } },
                checklist = new List<CheckList>() { new CheckList { value = "check1", IsChecked = true } }

            };
            //Act
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
            var okResponse = await notesController.DeleteNote(2) as OkObjectResult;
            //Act
            var result = okResponse.Value as Note;
            
            //Assert
            // Assert.IsType<OkResult>(okResponse);
            Assert.Equal(2, result.ID);


        }


        /// <summary>
        /// Put Test
        /// </summary>
        [Fact]
        public async void Test_Put()
        {
            var notePut = new Note
            {
                ID = 3,
                title = "Note Changed",
                plainText = "This is my plaintextttt",
                IsPinned = true,
                label = new List<Labels>() { new Labels { value = "Label_1" }, new Labels { value = "Label_2" } },
                checklist = new List<CheckList>() { new CheckList { value = "check1", IsChecked = true } }

            };
            var okResponse = await notesController.PutNote(3,notePut) as OkObjectResult;
            var result = okResponse.Value as Note;
            Assert.Equal("Note Changed", result.title);

        }

    }
}
