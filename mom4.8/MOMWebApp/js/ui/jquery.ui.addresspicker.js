/*
* jQuery UI addresspicker @VERSION
*
* Copyright 2010, AUTHORS.txt (http://jqueryui.com/about)
* Dual licensed under the MIT or GPL Version 2 licenses.
* http://jquery.org/license
*
* http://docs.jquery.com/UI/Progressbar
*
* Depends:
*   jquery.ui.core.js
*   jquery.ui.widget.js
*   jquery.ui.autocomplete.js
*/
(function($, undefined) {

    $.widget("ui.addresspicker", {
        options: {
            appendAddressString: "",
            mapOptions: {
                zoom: 14,
                center: new google.maps.LatLng(40, -101),
                scrollwheel: false,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            },
            elements: {
                map: false,
                lat: false,
                lng: false,
                locality: false,
                country: false
            },
            draggableMarker: true
        },

        marker: function() {
            return this.gmarker;
        },

        map: function() {
            return this.gmap;
        },

        updatePosition: function() {
            this._updatePosition(this.gmarker.getPosition());
        },

        reloadPosition: function() {
            this.gmarker.setVisible(true);
            this.gmarker.setPosition(new google.maps.LatLng(this.lat.val(), this.lng.val()));
            this.gmap.setCenter(this.gmarker.getPosition());
        },

        selected: function() {
            return this.selectedResult;
        },

        _create: function() {
            this.geocoder = new google.maps.Geocoder();

            this.element.autocomplete({
                source: $.proxy(this._geocode, this),
                focus: $.proxy(this._focusAddress, this),
                select: $.proxy(this._selectAddress, this)
                //                focus: function(event, ui) {
                //                
                //                    var str_array = ui.item.label.split(',');
                //                    for (var i = 0; i < str_array.length; i++) {
                //                        str_array[i] = str_array[i].replace(/^\s*/, "").replace(/\s*$/, "");
                //                    }
                //                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_tpLocation_txtAddress').value = str_array[0];
                //                    
                //                    return false;
                //                },
                //                select: function(event, ui) {
                //                
                //                    var str_array = ui.item.label.split(',');
                //                    for (var i = 0; i < str_array.length; i++) {
                //                        str_array[i] = str_array[i].replace(/^\s*/, "").replace(/\s*$/, "");
                //                    }
                //                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_tpLocation_txtAddress').value = str_array[0];
                //                    if (str_array.length > 3) {
                //                        document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_tpLocation_txtCity').value = str_array[str_array.length - 3];
                //                        document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_tpLocation_ddlState').value = str_array[str_array.length - 2].substring(0, 2);
                //                        document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_tpLocation_txtZip').value = str_array[str_array.length - 2].substring(2);
                //                    }
                //                    
                //                    return false;
                //                }
            });

            this.lat = $(this.options.elements.lat);
            this.lng = $(this.options.elements.lng);

            this.locality = $(this.options.elements.locality);
            this.country = $(this.options.elements.country);
            if (this.options.elements.map) {
                this.mapElement = $(this.options.elements.map);
                this._initMap();
            }
        },

        _initMap: function() {
            if (this.lat && this.lat.val()) {
                this.options.mapOptions.center = new google.maps.LatLng(this.lat.val(), this.lng.val());
            }

            this.gmap = new google.maps.Map(this.mapElement[0], this.options.mapOptions);
            this.gmarker = new google.maps.Marker({
                position: this.options.mapOptions.center,
                map: this.gmap,
                draggable: this.options.draggableMarker
            });
            google.maps.event.addListener(this.gmarker, 'dragend', $.proxy(this._markerMoved, this));
            this.gmarker.setVisible(false);
                                
            google.maps.event.addListenerOnce(this.gmap, 'idle', function() {
                $("#map").css({
                    display: 'none'
                });
            });
        },

        _updatePosition: function(location) {
            if (this.lat) {
                if (location.lat() != 40)                    
                this.lat.val(location.lat());
            }
            if (this.lng) {
                if (location.lng() != -101)
                    this.lng.val(location.lng());
            }            
        },

        _markerMoved: function() {
            this._updatePosition(this.gmarker.getPosition());
        },

        // Autocomplete source method: fill its suggests with google geocoder results
        _geocode: function(request, response) {
            var address = request.term, self = this;
            this.geocoder.geocode({ 'address': address + this.options.appendAddressString }, function(results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    for (var i = 0; i < results.length; i++) {
                        results[i].label = results[i].formatted_address;
                    };
                }
                response(results);
            })
        },

        _findInfo: function(result, type) {
            for (var i = 0; i < result.address_components.length; i++) {
                var component = result.address_components[i];
                if (component.types.indexOf(type) != -1) {
                    return component.long_name;
                }
            }
            return false;
        },

        _focusAddress: function(event, ui) {
            //            var address = ui.item;
            //            if (!address) {
            //                return;
            //            }

            //            if (this.gmarker) {
            //                this.gmarker.setPosition(address.geometry.location);
            //                this.gmarker.setVisible(true);

            //                this.gmap.fitBounds(address.geometry.viewport);
            //            }
            //            this._updatePosition(address.geometry.location);

            //            if (this.locality) {
            //                this.locality.val(this._findInfo(address, 'locality'));
            //            }
            //            if (this.country) {
            //                this.country.val(this._findInfo(address, 'country'));
            //            }

            var str_array = ui.item.label.split(',');
            for (var i = 0; i < str_array.length; i++) {
                str_array[i] = str_array[i].replace(/^\s*/, "").replace(/\s*$/, "");
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_tpLocation_txtAddress').value = str_array[0];
            return false;
        },

        _selectAddress: function(event, ui) {
            //        this.selectedResult = ui.item;
            var address = ui.item;
            var address = ui.item;
            if (!address) {
                return;
            }

            if (this.gmarker) {
                this.gmarker.setPosition(address.geometry.location);
                this.gmarker.setVisible(true);

                this.gmap.fitBounds(address.geometry.viewport);
                this.gmap.setZoom(18);                   
            }
            this._updatePosition(address.geometry.location);
            this.gmap.setZoom(18);          

            var str_array = ui.item.label.split(',');
            for (var i = 0; i < str_array.length; i++) {
                str_array[i] = str_array[i].replace(/^\s*/, "").replace(/\s*$/, "");
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_tpLocation_txtAddress').value = str_array[0];
            if (str_array.length > 3) {
                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_tpLocation_txtCity').value = str_array[str_array.length - 3];
                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_tpLocation_ddlState').value = str_array[str_array.length - 2].substring(0, 2);
                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_tpLocation_txtZip').value = str_array[str_array.length - 2].substring(2);
            }
            return false;
        }
    });

    $.extend($.ui.addresspicker, {
        version: "@VERSION"
    });


    // make IE think it doesn't suck
    if (!Array.indexOf) {
        Array.prototype.indexOf = function(obj) {
            for (var i = 0; i < this.length; i++) {
                if (this[i] == obj) {
                    return i;
                }
            }
            return -1;
        }
    }

})(jQuery);
