using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreFront2.DATA.EF;
using System.Drawing;
using StoreFront2.UI.MVC.Utilities;
using StoreFront2.UI.MVC.Models;

namespace StoreFront2.UI.MVC.Controllers
{
    public class JeweleriesController : Controller
    {
        private StoreFrontEntities db = new StoreFrontEntities();

        // GET: Jeweleries
        public ActionResult Index()
        {
            var jeweleries = db.Jeweleries.Include(j => j.Fit).Include(j => j.InvStatu).Include(j => j.Material).Include(j => j.Supplier).Include(j => j.Type);
            return View(jeweleries.ToList());
        }

        // GET: Jeweleries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jewelery jewelery = db.Jeweleries.Find(id);
            if (jewelery == null)
            {
                return HttpNotFound();
            }
            return View(jewelery);
        }

        // GET: Jeweleries/Create
        public ActionResult Create()
        {
            ViewBag.FitID = new SelectList(db.Fits, "FitID", "FitName");
            ViewBag.InvID = new SelectList(db.InvStatus, "InvID", "InvName");
            ViewBag.MaterialID = new SelectList(db.Materials, "MaterialID", "MaterialName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "TypeName");
            return View();
        }

        // POST: Jeweleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,MaterialID,FitID,InvID,TypeID,SupplierID,UnitsSold,ReleaseDate,ProductImage,Description,SoldAsPair,Price")] Jewelery jewelery, HttpPostedFileBase productCover)
        {
            if (ModelState.IsValid)
            {
                string file = "NoImage.jpg";

                if (productCover != null)
                {
                    file = productCover.FileName;
                    string ext = file.Substring(file.LastIndexOf('.'));
                    string[] goodExts =
                    {
                        ".jpg", ".jpeg", ".png", ".gif"
                    };
                    if (goodExts.Contains(ext.ToLower()) && productCover.ContentLength <= 4194304)
                    {
                        //create a new file name using GUID
                        file = Guid.NewGuid() + ext;

                        #region Resize Image
                        string savePath = Server.MapPath("~/Content/images/products/");
                        Image convertedImage = Image.FromStream(productCover.InputStream);

                        int maxImageSize = 500;
                        int maxThumbSize = 100;

                        ImageService.ResizeImage(savePath, file, convertedImage, maxImageSize, maxThumbSize);
                        #endregion
                    }



                }
                jewelery.ProductImage = file;//this for now needs to be outside the IF block, because it was manipulating the productCover / file
                //only. it wasn't even touching jewelery.ProductImage yet.

                db.Jeweleries.Add(jewelery);
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            ViewBag.FitID = new SelectList(db.Fits, "FitID", "FitName", jewelery.FitID);
            ViewBag.InvID = new SelectList(db.InvStatus, "InvID", "InvName", jewelery.InvID);
            ViewBag.MaterialID = new SelectList(db.Materials, "MaterialID", "MaterialName", jewelery.MaterialID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", jewelery.SupplierID);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "TypeName", jewelery.TypeID);
            return View(jewelery);
        }

        // GET: Jeweleries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jewelery jewelery = db.Jeweleries.Find(id);
            if (jewelery == null)
            {
                return HttpNotFound();
            }
            ViewBag.FitID = new SelectList(db.Fits, "FitID", "FitName", jewelery.FitID);
            ViewBag.InvID = new SelectList(db.InvStatus, "InvID", "InvName", jewelery.InvID);
            ViewBag.MaterialID = new SelectList(db.Materials, "MaterialID", "MaterialName", jewelery.MaterialID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", jewelery.SupplierID);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "TypeName", jewelery.TypeID);
            return View(jewelery);
        }

        // POST: Jeweleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,MaterialID,FitID,InvID,TypeID,SupplierID,UnitsSold,ReleaseDate,ProductImage,Description,SoldAsPair,Price")] Jewelery jewelery, HttpPostedFileBase productImage)//so the first productImage is the hiddenfor, then the input is this httppostedfilebase.

        {
            if (ModelState.IsValid)
            {
                #region file upload
                string file = jewelery.ProductImage;
                if (productImage != null)
                {
                    file = productImage.FileName;
                    string ext = file.Substring(file.LastIndexOf("."));//it knows to go to the very end since there is only the one param.
                    string[] goodExts =
                    {
                        ".jpeg", ".jpg", ".png", ".gif"
                    };
                    if (goodExts.Contains(ext.ToLower()) && productImage.ContentLength <= 4194304)//by default only 4mb.
                    {
                        file = Guid.NewGuid() + ext;//renaming the file name to a generate guid AND the ext we created.
                        #region resize image
                        string savePath = Server.MapPath("~/Content/images/products/");//map path gives us the file path for the certain environment.
                        Image ConvertedImage = Image.FromStream(productImage.InputStream);
                        int maxImageSize = 500;
                        int maxThumbSize = 100;
                        ImageService.ResizeImage(savePath, file, ConvertedImage, maxImageSize, maxThumbSize);
                        #endregion
                        if (jewelery.ProductImage != null && jewelery.ProductImage != "NoImage.jpg")//Noimage needs to be in products folder.
                        {
                            //if the image wasn't null, or was NoImage then:
                            ImageService.Delete(savePath, jewelery.ProductImage);
                        }
                        jewelery.ProductImage = file;
                    }


                }
                #endregion
                db.Entry(jewelery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FitID = new SelectList(db.Fits, "FitID", "FitName", jewelery.FitID);
            ViewBag.InvID = new SelectList(db.InvStatus, "InvID", "InvName", jewelery.InvID);
            ViewBag.MaterialID = new SelectList(db.Materials, "MaterialID", "MaterialName", jewelery.MaterialID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", jewelery.SupplierID);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "TypeName", jewelery.TypeID);
            return View(jewelery);
        }
        //AJAX DELETE STEP 2
        //// GET: Jeweleries/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Jewelery jewelery = db.Jeweleries.Find(id);
        //    if (jewelery == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(jewelery);
        //}

        //// POST: Jeweleries/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Jewelery jewelery = db.Jeweleries.Find(id);
        //    db.Jeweleries.Remove(jewelery);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

            //AJAX DELETE STEP 9 - Delete. Delete a jewelery record and return ONLY JSON data. Back to C# in a controller.
            [AcceptVerbs(HttpVerbs.Post)]//validation
            public JsonResult AjaxDelete(int id)//JsonResult is used to send Json-formatted content as a response.
        {
            Jewelery jewelery = db.Jeweleries.Find(id);//here jewelery is an object of jewelery class, and it equals the Jeweleries in the DB with the id we are seeking for the delete.
            db.Jeweleries.Remove(jewelery);//removes jewelery that was declared above that has the id we are seeking to delete. 
            db.SaveChanges();

            string confirmMessage = string.Format("Deleted Jewelery '{0}' from the database!", jewelery.ProductName); //here we called the targeted 
            //jewelery by name and returned a message to the user end telling them it was deleted.
            return Json(new { id, message = confirmMessage });//here is how we send back the message we created. Using Json, passing it to JS.
        }

        //AJAX DETAILS STEP 13 -- Details
        //gets a partial view for details with AJAX for display
        //Generate the Partial View with Details scaffolding for Publisher and we will check the box for Partial View
        //[HttpGet]
        //public PartialViewResult ProductDetails(int id)
        //{
        //    Jewelery jewelery = db.Jeweleries.Find(id);
        //    return PartialView(jewelery);
        //}


        public ActionResult AddToCart(int qty, int productID)//we are creating the AddToCart method here.
        {
            //create an empty shell for the LOCAL shopping cart variable.
            Dictionary<int, CartItemViewModel> shoppingCart = null;//made a dictionary called shopping cart. Its KEY is an int (qty) and its 
            //value is the class of CartItemViewModel.

            //check if the session shopping cart exists, if so, use it to populate values into the local shopping cart variable.
            if (Session["cart"] != null)//session is built into the tool base .NET Framework
            {
                //session cart does exist and we need to unbox it.
                shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];//we are casting explicit. SO HERE, SESSION CART IS NOW
                //THE VALUE OF DICTIONARY: SHOPPING CART.
            }
            else
            {
                //if session at the index of "cart" doesn't exist then we will new up an empty dictionary (initializing the collection)
                shoppingCart = new Dictionary<int, CartItemViewModel>();

            }
            //find the product that the user is adding to their cart, in this case it would be jeweleries.
            Jewelery product = db.Jeweleries.Where(j => j.ProductID == productID).FirstOrDefault();//the first or default ensures we only get 1
            //product back. //Product is being created here, and its in the db of jeweleries where the product id == product id. 

            //Now for defensive programming.
            if (product == null)
            //we got a bad id, we need to resend the index view for them to try again or make a different selection.
            {
                return RedirectToAction("Index");//return means sending something back to client //redirect to action means stop running current
                //method, find the ActionResult of Index() and run it.
            }
            else
            {
                //we were able to find a jewlery with the id passed to this method
                CartItemViewModel item = new CartItemViewModel(qty, product);

                //we are going to put item in the local shopping cart variable. BUT if we already have 1 of this product in the cart then it should
                //just update quantity.
                if (shoppingCart.ContainsKey(product.ProductID))//if shoppingCart already contains that key of product.ProductID
                {
                    shoppingCart[product.ProductID].Qty += qty; //if 1 was there before then reassign, or add to current qty.
                }
                else//this is if it wasn't already in there.
                {
                    shoppingCart.Add(product.ProductID, item);
                }

                //now we need to update the Session so that we can persist the info in the cart between request and response cycles.
                Session["cart"] = shoppingCart;//implicit casting. boxing up into a Session. ASK ABOUT THIS. Session is an Array List. Shopping
                                               //cart is a dictionary. Dictionaries have key value pairs. Like word and definition.

                Session["confirm"] = $"'{product.ProductName}' (Quantity: {qty}) added to cart";//we created a new array named confirm.

            }
            //send the user to the index of the shopping cart controller.
            return RedirectToAction("Index", "ShoppingCart");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }//end Dispose
    }//end class
}//end namespace
