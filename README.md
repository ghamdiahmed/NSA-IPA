# NSA-IPI: Non-Specific Amplification Implicated Primer Identification

## Overview

NSA-IPI is an online tool designed to assist researchers in identifying primer pairs responsible for non-specific amplifications (NSAs) in multiplex PCR settings. The tool streamlines experimental troubleshooting, enabling the accurate identification of NSA-causing primers and providing actionable insights for resolving amplification issues.

## Features

- Identification of NSA-causing primer pairs in multiplex PCR.
- Mis-priming prediction using embedded MegaBLAST analysis.
- Intuitive interface for inputting primer data and interpreting results.
- Recommendations for primer re-design or separation into sub-pools.

## Online Accessibility

The NSA-IPI tool is available online for immediate use. Access the tool here: [NSA-IPI Online Tool](http://www.nsa-ipi.online)

---

## Installation Instructions

To run NSA-IPI locally, follow these steps:

### Prerequisites
1. **System Requirements**:
   - Windows operating system.
   - Minimum 8 GB of RAM.
   - .NET Core Runtime installed (version 8.0 or higher).

### Steps
1. Clone the NSA-IPI repository:
   ```bash
   git clone [https://github.com/your-repository/NSA-IPI.git](https://github.com/ghamdiahmed/NSA-IPA.git)
   ```

2. Navigate to the project directory:
   ```bash
   cd NSA-IPI
   ```

3. Install the required .NET Core framework.

4. Build and run the application:
   ```bash
   dotnet run
   ```

5. Access the tool:
   Once the application is running, navigate to `http://localhost:5000` in your web browser to use NSA-IPI locally.

---

## License
NSA-IPI is licensed under the GNU General Public License v3.0. See the [LICENSE](./LICENSE.txt) file for details.
