
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;
using sportprofiles.Models.Contacts;

namespace sportprofiles.Services
{
    public class Contacts : IContacts
    {

        public Contacts()
        {
            
        }

        /// <summary>
        /// get my connections.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContactsModel>> GetMyContacts()
        {

            //simulate an async operation (e.g. data fetch from a DB or API)
            await Task.Delay(1000); //simulate a delay

            //Create and populate the list with data
            List<ContactsModel> lst = new List<ContactsModel>
            {
                new() {
                    ContactID = "1",
                    TitleDesc = "Pro Baseball Player",
                    PicturePath ="profile.png",
                    FriendName = "Barry Bonds"
                },
                new() {
                    ContactID = "2",
                    TitleDesc = "Golf Player",
                    PicturePath ="profile.png",
                    FriendName = "Tiger Woods"
                },
                new() {
                    ContactID = "3",
                    TitleDesc = "Tennis",
                    PicturePath ="profile.png",
                    FriendName = "John McEnroe"
                },
                new() {
                   ContactID = "4",
                    TitleDesc = "Basketball Player",
                    PicturePath ="profile.png",
                    FriendName = "Magic Johnson"
                },
                new() {
                  ContactID = "5",
                    TitleDesc = "Basketball Coach",
                    PicturePath ="profile.png",
                    FriendName = "Steve Kerr"
                },
                new() {
                   ContactID = "6",
                    TitleDesc = "Football Player",
                    PicturePath ="profile.png",
                    FriendName = "Tom Brady"
                }
            };
            return lst;

        }

        
        /// <summary>
        /// get connection requests.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContactsModel>> GetContactRequests()
        {
             //simulate an async operation (e.g. data fetch from a DB or API)
            await Task.Delay(1000); //simulate a delay

            //Create and populate the list with data
            List<ContactsModel> lst = new List<ContactsModel>
            {
                new() {
                    ContactID = "1",
                    TitleDesc = "Pro Baseball Player",
                    PicturePath ="profile.png",
                    FriendName = "John Smolts"
                },
                new() {
                    ContactID = "2",
                    TitleDesc = "Basketball",
                    PicturePath ="profile.png",
                    FriendName = "Asia Wilson"
                },
                new() {
                    ContactID = "3",
                    TitleDesc = "Tennis",
                    PicturePath ="profile.png",
                    FriendName = "Serena Williams"
                }
            };
            return lst;

        }

        /// <summary>
        /// get search result.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="jwtToken"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<ContactsModel> GetSearchResult()
        {
        
            //Create and populate the list with data
            List<ContactsModel> lst = new List<ContactsModel>
            {
                new() {
                    ContactID = "1",
                    TitleDesc = "Pro Baseball Player",
                    PicturePath ="profile.png",
                    FriendName = "John Smolts"
                },
                new() {
                    ContactID = "2",
                    TitleDesc = "Basketball",
                    PicturePath ="profile.png",
                    FriendName = "Asia Wilson"
                },
                new() {
                    ContactID = "3",
                    TitleDesc = "Tennis",
                    PicturePath ="profile.png",
                    FriendName = "Serena Williams"
                }
            };
            return lst;
        }

    }
        
    public interface IContacts
    {
        Task<List<ContactsModel>> GetMyContacts();
        Task<List<ContactsModel>> GetContactRequests();
        List<ContactsModel> GetSearchResult();

    }
}
