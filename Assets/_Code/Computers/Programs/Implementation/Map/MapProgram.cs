using HASH;
using UnityEngine;

namespace HASH
{
    public static class MapProgram
    {
        public const float OneDegreeInKm = 111f;
        
        public const string LatitudeParameterName = "lat";
        public const string LongitudeParameterName = "lon";

        public static CommandLineArgValidationOption[] Validations;
        public static CommandLineArgValidationOption LatitudeValidation;
        public static CommandLineArgValidationOption LongitudeValidation;

        public static MapAditionalData AditionalData;

        public static void Setup(Object aditionalData)
        {
            AditionalData = aditionalData as MapAditionalData;
            DebugUtil.Assert(AditionalData == null, "MapProgram's aditional data is not a MapAditionalData!");

            LatitudeValidation = new CommandLineArgValidationOption();
            LatitudeValidation.ArgumentName = LatitudeParameterName;
            LatitudeValidation.Requirements = ArgRequirement.Required | ArgRequirement.ValueRequired;

            LongitudeValidation = new CommandLineArgValidationOption();
            LongitudeValidation.ArgumentName = LongitudeParameterName;
            LongitudeValidation.Requirements = ArgRequirement.Required | ArgRequirement.ValueRequired;

            Validations = new[] {LatitudeValidation, LongitudeValidation};
        }

        public static void Execute(ProgramExecutionOptions options)
        {
            if (ProgramUtil.ShowHelpIfNeeded(options))
                return;

            if (CommandLineUtil.ValidateArguments(options.ParsedArguments, Validations))
            {
                var latitudeParam = CommandLineUtil.FindArgumentByName(options.ParsedArguments, LatitudeParameterName);
                var longitudeParam = CommandLineUtil.FindArgumentByName(options.ParsedArguments, LongitudeParameterName);

                var latitude = latitudeParam.Value;
                var longitude = longitudeParam.Value;

                float fLatitude;
                float fLongitude;
                if (ValidateCoords(latitude, longitude, out fLatitude, out fLongitude))
                {
                    float imageWidth = AditionalData.MapTexture.width;
                    float imageHeight = AditionalData.MapTexture.height;
                    
                    var markerPositon = new Vector2(fLatitude, fLongitude);

                    var dimentions = AditionalData.MapDimentions;
                    if (dimentions.Contains(markerPositon))
                    {
                        string title = string.Format("Map for lat: {0:F3} / lon: {1:F3}", fLatitude, fLongitude);
                        var imageWindow = WindowUtil.CreateImageWindow(AditionalData.MapTexture, title);

                        var holder = imageWindow.MainWidget;
                        var markerInstance = NGUITools.AddChild(holder.gameObject, AditionalData.MarkerGameObject);

                        markerInstance.transform.localPosition = GetMarkerPosition(AditionalData.MapTexture, dimentions, markerPositon);
                    }
                    else
                    {
                        var fromDimention = string.Format("[lat {0} / lon {1}]", dimentions.xMin, dimentions.yMin);
                        var toDimention = string.Format("[lat {0} / lon {1}]", dimentions.xMax, dimentions.yMax);
                        var msg = "The given positon lies outside the available map. The map data encloses goes from {0} to {1}";
                        msg = string.Format(msg, fromDimention, toDimention);
                        msg = TextUtil.Error(msg);
                        TerminalUtil.ShowText(msg);
                    }
                }
                else
                {
                    var msg = "Invalid coordinates. Please supply latitude and longitude as decimal numers.\nEx: lat 25.6939 log 91.9057";
                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                }
            }
            else
            {
                var msg = "Error. Please supply a valid latitude and longitude";
                msg = TextUtil.Error(msg);
                TerminalUtil.ShowText(msg);
            }
        }

        public static bool ValidateCoords(string latitude, string longitude, out float fLatitude, out float fLongitude)
        {
            return float.TryParse(latitude, out fLatitude) & float.TryParse(longitude, out fLongitude);
        }

        public static Vector2 GetMarkerPosition(Texture2D image, Rect mapDimenttions, Vector2 desiredPosition)
        {
            var result = new Vector2();

            var desiredPosAbs = new Vector2(Mathf.Abs(desiredPosition.x), Mathf.Abs(desiredPosition.y));
            var mapPosAbs = new Vector2(Mathf.Abs(mapDimenttions.x), Mathf.Abs(mapDimenttions.y));

            float normalizedX = Mathf.Abs((desiredPosAbs.x - mapPosAbs.x) / mapDimenttions.width);
            float normalizedY = Mathf.Abs((desiredPosAbs.y - mapPosAbs.y) / mapDimenttions.height);

            normalizedX = Mathf.Lerp(-1, 1, normalizedX);
            normalizedY = Mathf.Lerp(-1, 1, normalizedY);

            // Remove 50 so that the marker is always inside the map
            var halfWidth = Mathf.Ceil((image.width - 40) / 2f);
            var halfHeight = Mathf.Ceil((image.height - 40) / 2f);
            return new Vector2(normalizedX * halfWidth, normalizedY * halfHeight);
        }
    }
}