﻿/* JS Document */

/******************************

[Table of Contents]

1. Vars and Inits
2. Set Header
3. Init Menu
4. Init Favorite
5. Init Fix Product Border
6. Init Isotope Filtering
7. Init Price Slider
8. Init Checkboxes



******************************/

jQuery(document).ready(function($)
{
	"use strict";

	/* 

	1. Vars and Inits

	*/

	var header = $('.header');
	var topNav = $('.top_nav')
	var mainSlider = $('.main_slider');
	var hamburger = $('.hamburger_container');
	var menu = $('.hamburger_menu');
	var menuActive = false;
	var hamburgerClose = $('.hamburger_close');
	var fsOverlay = $('.fs_menu_overlay');

	setHeader();

	$(window).on('resize', function()
	{
		initFixProductBorder();
		setHeader();
	});

	$(document).on('scroll', function()
	{
		setHeader();
	});

	initMenu();
	initFavorite();
	initFixProductBorder();
	initIsotopeFiltering();
	initPriceSlider();
	initCheckboxes();

	/* 

	2. Set Header

	*/

	function setHeader()
	{
		if(window.innerWidth < 992)
		{
			if($(window).scrollTop() > 100)
			{
				header.css({'top':"0"});
			}
			else
			{
				header.css({'top':"0"});
			}
		}
		else
		{
			if($(window).scrollTop() > 100)
			{
				header.css({'top':"-50px"});
			}
			else
			{
				header.css({'top':"0"});
			}
		}
		if(window.innerWidth > 991 && menuActive)
		{
			closeMenu();
		}
	}

	/* 

	3. Init Menu

	*/

	function initMenu()
	{
		if(hamburger.length)
		{
			hamburger.on('click', function()
			{
				if(!menuActive)
				{
					openMenu();
				}
			});
		}

		if(fsOverlay.length)
		{
			fsOverlay.on('click', function()
			{
				if(menuActive)
				{
					closeMenu();
				}
			});
		}

		if(hamburgerClose.length)
		{
			hamburgerClose.on('click', function()
			{
				if(menuActive)
				{
					closeMenu();
				}
			});
		}

		if($('.menu_item').length)
		{
			var items = document.getElementsByClassName('menu_item');
			var i;

			for(i = 0; i < items.length; i++)
			{
				if(items[i].classList.contains("has-children"))
				{
					items[i].onclick = function()
					{
						this.classList.toggle("active");
						var panel = this.children[1];
					    if(panel.style.maxHeight)
					    {
					    	panel.style.maxHeight = null;
					    }
					    else
					    {
					    	panel.style.maxHeight = panel.scrollHeight + "px";
					    }
					}
				}	
			}
		}
	}

	function openMenu()
	{
		menu.addClass('active');
		// menu.css('right', "0");
		fsOverlay.css('pointer-events', "auto");
		menuActive = true;
	}

	function closeMenu()
	{
		menu.removeClass('active');
		fsOverlay.css('pointer-events', "none");
		menuActive = false;
	}

	/* 

	4. Init Favorite

	*/

    function initFavorite()
    {
    	if($('.favorite').length)
    	{
    		var favs = $('.favorite');

    		favs.each(function()
    		{
    			var fav = $(this);
    			var active = false;
    			if(fav.hasClass('active'))
    			{
    				active = true;
    			}

    			fav.on('click', function()
    			{
    				if(active)
    				{
    					fav.removeClass('active');
    					active = false;
    				}
    				else
    				{
    					fav.addClass('active');
    					active = true;
    				}
    			});
    		});
    	}
    }

    /* 

	5. Init Fix Product Border

	*/

    function initFixProductBorder()
    {
    	if($('.product_filter').length)
    	{
			var products = $('.product_filter:visible');
    		var wdth = window.innerWidth;

    		// reset border
    		products.each(function()
    		{
    			$(this).css('border-right', 'solid 1px #e9e9e9');
    		});

    		// if window width is 991px or less

    		if(wdth < 480)
			{
				for(var i = 0; i < products.length; i++)
				{
					var product = $(products[i]);
					product.css('border-right', 'none');
				}
			}

    		else if(wdth < 576)
			{
				if(products.length < 5)
				{
					var product = $(products[products.length - 1]);
					product.css('border-right', 'none');
				}
				for(var i = 1; i < products.length; i+=2)
				{
					var product = $(products[i]);
					product.css('border-right', 'none');
				}
			}

    		else if(wdth < 768)
			{
				if(products.length < 5)
				{
					var product = $(products[products.length - 1]);
					product.css('border-right', 'none');
				}
				for(var i = 2; i < products.length; i+=3)
				{
					var product = $(products[i]);
					product.css('border-right', 'none');
				}
			}

    		else if(wdth < 992)
			{
				if(products.length < 5)
				{
					var product = $(products[products.length - 1]);
					product.css('border-right', 'none');
				}
				for(var i = 2; i < products.length; i+=3)
				{
					var product = $(products[i]);
					product.css('border-right', 'none');
				}
			}

			//if window width is larger than 991px
			else
			{
				if(products.length < 5)
				{
					var product = $(products[products.length - 1]);
					product.css('border-right', 'none');
				}
				for(var i = 3; i < products.length; i+=4)
				{
					var product = $(products[i]);
					product.css('border-right', 'none');
				}
			}	
    	}
    }

    /* 

	6. Init Isotope Filtering

	*/

    function initIsotopeFiltering()
    {
    	var sortTypes = $('.type_sorting_btn');
    	var sortNums = $('.num_sorting_btn');
    	var sortTypesSelected = $('.sorting_type .item_sorting_btn is-checked span');
    	var filterButton = $('.filter_button');
    	var sumOfProducts = $('.product-grid').length;

    	if($('.product-grid').length)
    	{
    		$('.product-grid').isotope({
    			itemSelector: '.product-item',
	            getSortData: {
	            	price: function(itemElement)
	            	{
						var priceEle = $(itemElement).find('.in_product_price').text();
						return parseFloat(priceEle);
	            	},
	            	name: '.product_name'
	            },
	            animationOptions: {
	                duration: 750,
	                easing: 'linear',
	                queue: false
	            }
	        });

    		// Short based on the value from the sorting_type dropdown
	        sortTypes.each(function()
	        {
	        	$(this).on('click', function()
	        	{
	        		$('.type_sorting_text').text($(this).text());
	        		var option = $(this).attr('data-isotope-option');
	        		option = JSON.parse( option );
    				$('.product-grid').isotope( option );
	        	});
	        });

	        // Show only a selected number of items
	        sortNums.each(function()
            	        {
            	        	$(this).on('click', function()
            	        	{
            	        		var numSortingText = $(this).text();
            					var numFilter = ':nth-child(-n+' + numSortingText + ')';
            	        		$('.num_sorting_text').text($(this).text());
							
                				$('.product-grid').isotope({filter: numFilter });
            					var page_total = $('.page_total');
            					var totalPagination = Math.ceil($('.product-item').length / numSortingText);
            					page_total.text(totalPagination);
            					var page_selection = $('.page_selection');
            					// add <li><a href="#">{}</a></li> to page_selection
            					// var page_selection_html = '';
            					// for (var i = 1; i <= totalPagination; i++) {
            					// 	page_selection_html += '<li><a href="#">' + i + '</a></li>';
            					// }
            					// page_selection.html(page_selection_html);
            
            					// set Request.QueryString["sizePage"] = numSortingText; in 
            					// set to url when click
            					var url = window.location.href;
            					// reset and add sizePage to url
            					var newUrl = url.split('?')[0]+'?sizePage='+parseInt(numSortingText)+'&page=1'  ;
            					// set url
            					window.history.pushState("object or string", "Title", newUrl);
								window.location.reload();
								
            	        	});
            	        });	

	        // Filter based on the price range slider
	        filterButton.on('click', function()
	        {
	        	$('.product-grid').isotope({
		            filter: function()
		            {
		            	/* var priceRange = $('#amount').val();*/
			        	var priceMin = parseInt($('#minval').val());
			        	var priceMax = parseInt($('#maxval').val());
						
						if(isNaN(priceMin) || isNaN(priceMax) ) {

							document.getElementById("err").style.color = "red";
							$('#err').text('Please choose price range');
							 var itemPrice = parseInt($(this).find('.in_product_price').clone().children().remove().end().text());
							 return (itemPrice > 0) && (itemPrice< 9999999999999);
							
						}
						else 
						{
							document.getElementById("err").textContent = "";
							var itemPrice = parseInt($(this).find('.in_product_price').clone().children().remove().end().text());
							return (itemPrice > priceMin) && (itemPrice < priceMax);
						
						
							
						}
		            },
		            animationOptions: {
		                duration: 750,
		                easing: 'linear',
		                queue: false
		            }
		        });
	        });
    	}
    }
    
    
    function initPagination(paginationNumber){
        // Initialize pagination
        var pagination = $('#pagination');
        
        if(paginationNumber > 1) {
        // create divs for pagination
        for(var i = 0; i < paginationNumber; i++) {
            var pageNum = i + 1;
            pagination.append('<div class="page_num" data-page="' + pageNum + '">' + pageNum + '</div>');
            
        }
        }
    }
    /* 

	7. Init Price Slider

	*/

    function initPriceSlider()
    {
		$( "#slider-range" ).slider(
		{
			range: true,
			min: 0,
			max: 2000000,
			values: [ 0, 500000 ],
			slide: function( event, ui )
			{
				$("#amount").val("đ" + ui.values[0] + " - đ" + ui.values[1]);
				$('#FromAmount').val(ui.values[0]);
				$('#ToAmount').val(ui.values[1]);
			}
		});
			
		$( "#amount" ).val( "đ" + $( "#slider-range" ).slider( "values", 0 ) + " - đ" + $( "#slider-range" ).slider( "values", 1 ) );
    }

    /* 

	8. Init Checkboxes

	*/

    function initCheckboxes()
    {
    	if($('.checkboxes li').length)
    	{
    		var boxes = $('.checkboxes li');

    		boxes.each(function()
    		{
    			var box = $(this);

    			box.on('click', function()
    			{
    				if(box.hasClass('active'))
    				{
    					box.find('i').removeClass('fa-square');
    					box.find('i').addClass('fa-square-o');
    					box.toggleClass('active');
    				}
    				else
    				{
    					box.find('i').removeClass('fa-square-o');
    					box.find('i').addClass('fa-square');
    					box.toggleClass('active');
    				}
    				// box.toggleClass('active');
    			});
    		});

    		if($('.show_more').length)
    		{
    			var checkboxes = $('.checkboxes');

    			$('.show_more').on('click', function()
    			{
    				checkboxes.toggleClass('active');
    			});
    		}
    	};
    }
});