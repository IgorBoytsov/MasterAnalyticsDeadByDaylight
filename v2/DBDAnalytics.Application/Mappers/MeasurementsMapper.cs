using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;
using System.Collections.ObjectModel;

namespace DBDAnalytics.Application.Mappers
{
    internal static class MeasurementsMapper
    {
        public static MeasurementDTO ToDTO(this MeasurementDomain measurement)
        {
            return new MeasurementDTO
            {
                IdMeasurement = measurement.IdMeasurement,
                MeasurementName = measurement.MeasurementName,
                MeasurementDescription = measurement.MeasurementDescription,
            };
        }

        public static List<MeasurementDTO> ToDTO(this IEnumerable<MeasurementDomain> measurements)
        {
            var list = new List<MeasurementDTO>();
            
            foreach (var measurement in measurements)
            {
                list.Add(measurement.ToDTO());
            }

            return list;
        }

        public static MeasurementWithMapsDTO ToDTO(this MeasurementWithMapsDomain measurementWithMaps)
        {
            var mapsDTO = new ObservableCollection<MapDTO>();

            foreach (var item in measurementWithMaps.Maps.ToDTO())
            {
                mapsDTO.Add(item);
            }

            return new MeasurementWithMapsDTO
            {
                IdMeasurement = measurementWithMaps.IdMeasurement,
                MeasurementName = measurementWithMaps.MeasurementName,
                MeasurementDescription = measurementWithMaps.MeasurementDescription,
                Maps = mapsDTO,
            };
        }

        public static List<MeasurementWithMapsDTO?> ToDTO(this IEnumerable<MeasurementWithMapsDomain> measurementsWithMaps)
        {
            var measurementsWithMapsDTO = new List<MeasurementWithMapsDTO?>();

            foreach (var measurementWithMaps in measurementsWithMaps)
            {
                var mapsDTO = new ObservableCollection<MapDTO>();

                foreach (var item in measurementWithMaps.Maps.ToDTO())
                {
                    mapsDTO.Add(item);
                }
                measurementsWithMapsDTO.Add(new MeasurementWithMapsDTO
                {
                    IdMeasurement = measurementWithMaps.IdMeasurement,
                    MeasurementName = measurementWithMaps.MeasurementName,
                    MeasurementDescription = measurementWithMaps.MeasurementDescription,
                    Maps = mapsDTO,
                });
            }

            return measurementsWithMapsDTO;
        }
    }
}