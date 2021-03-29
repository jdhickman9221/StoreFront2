using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreFront2.DATA.EF;//adding for access to Domain Models (Jeweleries)
using System.ComponentModel.DataAnnotations;//added for validation / display data annotations.

namespace StoreFront2.UI.MVC.Models
{
    //Shopping cart step 1
    //View Model vs. Domain Model
    //Domain model is a class tied to a DB structure either through EF or Connected SQL
    //View model is not tied to a DB structure. 
    //both are classes, they have properties, fields, constructors, etc. 
    //REMEMBER THAT: normalization is the process of creating look up tables etc, to reduce redundancy of info. 
    //Why would we make a view model? Because not all data needs to be persisted. and sometimes you need to perform the calculation
    //in the middle tier because you don't want to have to update this info in the DB when it could just be figured out
    // by calculations.
    //view model cs domain model.

    public class CartItemViewModel

    //using automatic properties syntax.
    //remember we only need the other props for business rules. 
    //getter for read only, setter for write only.

    {
        //no fields using automatic properties.
        [Range(1, int.MaxValue)]//this validation ensures the values are greater than 0 but less than the max size for the datatype.
        public int Qty { get; set; }

        public Jewelery Product { get; set; }//Containment: when a complex datatype has another complex datatype as a property. (HAS a type replationship)

        //now its time for the FQ Ctor
        public CartItemViewModel(int qty, Jewelery product)
        {
            Product = product;
            Qty = qty;
        }
    }
}