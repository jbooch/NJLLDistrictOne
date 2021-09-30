var MenuDef = {
	"type"	 : "bar",
	"style"	 : {
		"css" : "xp",
		"box"	 : true,
		"size"	 : [250, 26],
		"bgcolor" : "#D3E5FA",
		"direction" : "v",
		"border" : {
			"color" : "#55A1FF", "size" : 1
		},
		"imgendon" : {
			"src" : "img/arr_on.gif", "width" : 5, "height" : 10
		},
		"imgendoff" : {
			"src" : "img/arr_off.gif", "width" : 5, "height" : 10
		},
		"imgdir" : {
			"src" : "img/dir.gif", "width" : 24, "height" : 24
		},
		"imgitem" : {
			"src" : "img/item.gif", "width" : 24, "height" : 24
		},
		"imgspace" : 5
	},
	"itemover" : {
		"css" : "xp", "bgcolor" : "#1665CB", "color" : "white",
		"imgdir" : {
			"src" : "img/dir_over.gif", "width" : 24, "height" : 24
		},
		"imgitem" : {
			"src" : "img/item_over.gif", "width" : 24, "height" : 24
		}
	},
	"separator" : {
		"style" : {
			"bgimg" : "img/separator.gif",
			"size" : [250, 2], "imgitem" : ""
		}
	},
	"position" : {
		"absolute" : false, "pos" : [30, 20]
	},
	"defaction" : {
		"title" : "This is an action title"
	},
	"items" : [
	{
		"text" : "Example of XP-style menu",
		"menu" : {
			"style" : {
				"direction" : "v",
				"bar" : {
					"src" : "img/bar_xp.gif",
					"size" : [6, 65]
				},
				"css" : "xp2",
				"bgcolor" : "white",
				"imgdir" : {
					"src" : "img/subdir.gif",
					"width" : 18, "height" : 16
				},
				"imgitem" : {
					"src" : "img/subitem.gif",
					"width" : 18, "height" : 16
				},
				"size" : [180, 20]
			},
			"separator" : {
				"style" : {
					"bgimg" : "img/separator.gif",
					"size" : [180, 2],
					"imgitem" : ""
				}
			},
			"itemover" : {
				"css" : "xp2", "bgcolor" : "#1665CB",
				"color" : "white",
				"imgdir" : {
					"src" : "img/subdir_over.gif",
					"width" : 18, "height" : 16
				},
				"imgitem" : {
					"src" : "img/subitem_over.gif",
					"width" : 18, "height" : 16
				}
			},
			"position" : {
				"anchor" : "ne",
				"anchor_side" : "nw"
			},
			"items"	 : [
			{
				"text" : "JS code",
				"action" : {
					"js" : "openWin(30, 30);"
				}
			},
			{
				"type" : "separator"
			},
			{
				"text" : "Go to URL",
				"menu" : {
					"separator" : {
						"style" : {
							"bgimg" :
							"img/" + 
							"separator.gif",
							"size" : [180, 2],
							"imgitem" : ""
						}
					},
					"style" : {
						"size" : [100, 20]
					},
					"items"	 : [
					{
						"text" : "Item1"
					},
					{
						"text" : "Item2"
					},
					{
						"type" : "separator"
					},
					{
						"text" : "Item3"
					} ]
				},
				"action" : {
					"url" : "http://yahoo.com",
					"target" : "_blank",
					"title" : "Yahoo!!!!"
				}
			},
			{
				"text" : "Boolean item",
				"type" : "bool",
				"action" : {
					"on" : "alert(\'on!\');",
					"off" : "alert(\'off!\');"
				}
			} ]
		}
	},
	{
		"text" : "Example of XP-style menu"
	},
	{
		"type" : "separator"
	},
	{
		"text" : "Example of XP-style menu"
	},
	{
		"text" : "Example of XP-style menu"
	} ]
}; 
