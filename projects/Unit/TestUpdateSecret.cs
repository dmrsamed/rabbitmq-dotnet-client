// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
//
// The APL v2.0:
//
//---------------------------------------------------------------------------
//   Copyright (c) 2007-2020 VMware, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       https://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//---------------------------------------------------------------------------
//
// The MPL v2.0:
//
//---------------------------------------------------------------------------
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
//
//  Copyright (c) 2007-2020 VMware, Inc.  All rights reserved.
//---------------------------------------------------------------------------

using System;
using System.Text;

using NUnit.Framework;

namespace RabbitMQ.Client.Unit
{
    [TestFixture]
    public class TestUpdateSecret : IntegrationFixture {

        [Test]
        public void TestUpdatingConnectionSecret()
        {
            if (!RabbitMQ380OrHigher())
            {
                Console.WriteLine("Not connected to RabbitMQ 3.8 or higher, skipping test");
                return;
            }

            _conn.UpdateSecret("new-secret", "Test Case");

            Assert.AreEqual("new-secret", _connFactory.Password);
        }

        private bool RabbitMQ380OrHigher()
        {
            System.Collections.Generic.IDictionary<string, object> properties = _conn.ServerProperties;

            if (properties.TryGetValue("version", out object versionVal))
            {
                string versionStr = Encoding.UTF8.GetString((byte[])versionVal);

                int dashIdx = versionStr.IndexOf('-');
                if (dashIdx > 0)
                {
                    versionStr = versionStr.Remove(dashIdx);
                }

                if (Version.TryParse(versionStr, out Version version))
                {
                    return version >= new Version(3, 8);
                }
            }

            return false;
        }
    }
}
