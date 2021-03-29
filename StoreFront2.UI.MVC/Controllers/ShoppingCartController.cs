//Shopping cart step 4 - creating controller.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreFront2.UI.MVC.Models;//gives us access to models.



namespace StoreFront2.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        //shopping cart - step 5 - update logic for Index()
        // GET: ShoppingCart
        //WE GENERATED A VIEW that was a list with a model of CartItemViewModel and no data context class.
        public ActionResult Index()
        {
            var shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];//explicit casting again. unboxing
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                //user either has nothing in cart or removed all items or session expired.
                //set cart to empty object. (can still send that to the view unlike null)
                shoppingCart = new Dictionary<int, CartItemViewModel>();
                //create a message about the empty cart
                ViewBag.Message = "There are no items in your cart.";
            }
            else
            {
                ViewBag.Message = null; //explicitly clears out ViewBag variable.

            }
            return View(shoppingCart);
            //now lets go change the index view.

        }
        //shopping cart step 7 - Update the cart usting form on index of this controller.
        public ActionResult UpdateCart(int productId, int qty)
        {
            //get the cart out of session and into a local variable
            var shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];

            //target the correct cartItem productID - then change the quantity for that item
            shoppingCart[productId].Qty = qty;//this is reassigning the qty. Qty = qty  while targeting the key of product in the dictionary.

            //update session with new cart info.
            Session["cart"] = shoppingCart;

            return RedirectToAction("Index");


        }
        public ActionResult RemoveFromCart(int id)
        {
            Dictionary<int, CartItemViewModel> shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];
            //session stores things of type object.

            shoppingCart.Remove(id);

            //update the session
            Session["cart"] = shoppingCart;

            return RedirectToAction("Index");
        }
    }
}