﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools
{
    public static class ListExtension
    {
        public static IEnumerable<string> ShouldContainAll(this IEnumerable<string> list, IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (string value in values)
            {
                list.Should().Contain(value);
            }

            return list;
        }

        public static IEnumerable<string> ShouldContainOnly(this IEnumerable<string> list, IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (string value in values)
            {
                list.Should().Contain(value);
            }

            list.Should().HaveCount(values.Count());

            return list;
        }

        public static IEnumerable<string> ShouldContain(this IEnumerable<string> list, string value)
        {
            list.Should().Contain(value);

            return list;
        }

        public static IEnumerable<string> ShouldNotContainAnyOf(this IEnumerable<string> list, IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (string value in values)
            {
                list.Should().NotContain(value);
            }

            return list;
        }


        public static IEnumerable<string> ShouldNotContain(this IEnumerable<string> list, string value)
        {
            list.Should().NotContain(value);

            return list;
        }
    }
}
