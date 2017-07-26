(function (root, factory) {
    if (typeof module === 'object' && module.exports) {
        module.exports = root.document ?
            factory(root) :
            factory;
    } else {
        root.Custom = factory(root);
    }
}(typeof window !== 'undefined' ? window : this, function (win) {
    var Custom = {};
    (function (H) {
        /**
         * Utility function to extend an object with the members of another.
         *
         * @function #extend
         * @memberOf Highcharts
         * @param {Object} a - The object to be extended.
         * @param {Object} b - The object to add to the first one.
         * @returns {Object} Object a, the original object.
         */
        H.extend = function (a, b) {
            var n;
            if (!a) {
                a = {};
            }
            for (n in b) {
                a[n] = b[n];
            }
            return a;
        };

        /**
         * Utility function to deep merge two or more objects and return a third object.
         * If the first argument is true, the contents of the second object is copied
         * into the first object. The merge function can also be used with a single 
         * object argument to create a deep copy of an object.
         *
         * @function #merge
         * @memberOf Highcharts
         * @param {Boolean} [extend] - Whether to extend the left-side object (a) or
                  return a whole new object.
         * @param {Object} a - The first object to extend. When only this is given, the
                  function returns a deep copy.
         * @param {...Object} [n] - An object to merge into the previous one.
         * @returns {Object} - The merged object. If the first argument is true, the 
         * return is the same as the second argument.
         */
        H.merge = function () {
            var i,
                args = arguments,
                len,
                ret = {},
                doCopy = function (copy, original) {
                    // An object is replacing a primitive
                    if (typeof copy !== 'object') {
                        copy = {};
                    }

                    H.objectEach(original, function (value, key) {

                        // Copy the contents of objects, but not arrays or DOM nodes
                        if (
                            H.isObject(value, true) &&
                            !H.isClass(value) &&
                            !H.isDOMElement(value)
                        ) {
                            copy[key] = doCopy(copy[key] || {}, value);

                            // Primitives and arrays are copied over directly
                        } else {
                            copy[key] = original[key];
                        }
                    });
                    return copy;
                };

            // If first argument is true, copy into the existing object. Used in
            // setOptions.
            if (args[0] === true) {
                ret = args[1];
                args = Array.prototype.slice.call(args, 2);
            }

            // For each argument, extend the return
            len = args.length;
            for (i = 0; i < len; i++) {
                ret = doCopy(ret, args[i]);
            }

            return ret;
        };

        /**
         * Utility function to check for string type.
         *
         * @function #isString
         * @memberOf Highcharts
         * @param {Object} s - The item to check.
         * @returns {Boolean} - True if the argument is a string.
         */
        H.isString = function (s) {
            return typeof s === 'string';
        };

        /**
         * Utility function to check if an item is an array.
         *
         * @function #isArray
         * @memberOf Highcharts
         * @param {Object} obj - The item to check.
         * @returns {Boolean} - True if the argument is an array.
         */
        H.isArray = function (obj) {
            var str = Object.prototype.toString.call(obj);
            return str === '[object Array]' || str === '[object Array Iterator]';
        };

        /**
         * Utility function to check if an item is of type object.
         *
         * @function #isObject
         * @memberOf Highcharts
         * @param {Object} obj - The item to check.
         * @param {Boolean} [strict=false] - Also checks that the object is not an
         *    array.
         * @returns {Boolean} - True if the argument is an object.
         */
        H.isObject = function (obj, strict) {
            return !!obj && typeof obj === 'object' && (!strict || !H.isArray(obj));
        };

        /**
         * Utility function to check if an Object is a HTML Element.
         *
         * @function #isDOMElement
         * @memberOf Highcharts
         * @param {Object} obj - The item to check.
         * @returns {Boolean} - True if the argument is a HTML Element.
         */
        H.isDOMElement = function (obj) {
            return H.isObject(obj) && typeof obj.nodeType === 'number';
        };

        /**
         * Utility function to check if an Object is an class.
         *
         * @function #isClass
         * @memberOf Highcharts
         * @param {Object} obj - The item to check.
         * @returns {Boolean} - True if the argument is an class.
         */
        H.isClass = function (obj) {
            var c = obj && obj.constructor;
            return !!(
                H.isObject(obj, true) &&
                !H.isDOMElement(obj) &&
                (c && c.name && c.name !== 'Object')
            );
        };

        /**
         * Utility function to check if an item is of type number.
         *
         * @function #isNumber
         * @memberOf Highcharts
         * @param {Object} n - The item to check.
         * @returns {Boolean} - True if the item is a number and is not NaN.
         */
        H.isNumber = function (n) {
            return typeof n === 'number' && !isNaN(n);
        };

        /**
         * Iterate over object key pairs in an object.
         *
         * @function #objectEach
         * @memberOf Highcharts
         * @param  {Object}   obj - The object to iterate over.
         * @param  {Function} fn  - The iterator callback. It passes three arguments:
         * * value - The property value.
         * * key - The property key.
         * * obj - The object that objectEach is being applied to.
         * @param  {Object}   ctx The context
         */
        H.objectEach = function (obj, fn, ctx) {
            for (var key in obj) {
                if (obj.hasOwnProperty(key)) {
                    fn.call(ctx, obj[key], key, obj);
                }
            }
        };

        /**
         * Non-recursive method to find the lowest member of an array. `Math.min` raises
         * a maximum call stack size exceeded error in Chrome when trying to apply more
         * than 150.000 points. This method is slightly slower, but safe.
         *
         * @function #arrayMin
         * @memberOf  Highcharts
         * @param {Array} data An array of numbers.
         * @returns {Number} The lowest number.
         */
        H.arrayMin = function (data) {
            var i = data.length,
                min = data[0];

            while (i--) {
                if (data[i] < min) {
                    min = data[i];
                }
            }
            return min;
        };

        /**
         * Non-recursive method to find the lowest member of an array. `Math.max` raises
         * a maximum call stack size exceeded error in Chrome when trying to apply more
         * than 150.000 points. This method is slightly slower, but safe.
         *
         * @function #arrayMax
         * @memberOf  Highcharts
         * @param {Array} data - An array of numbers.
         * @returns {Number} The highest number.
         */
        H.arrayMax = function (data) {
            var i = data.length,
                max = data[0];

            while (i--) {
                if (data[i] > max) {
                    max = data[i];
                }
            }
            return max;
        };
    }(Custom));
    return Custom;
}));